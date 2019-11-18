import next from 'next';
import { DEV, SERVER_HOST, SERVER_URL, SERVER_PORT_HTTP, SERVER_PORT_HTTPS } from '../src/constants/env';
import { readFileSync } from 'fs';
import http from 'http';
import bodyParser from 'body-parser';
import https from 'https';
import hsts from 'hsts';
import url from 'url';
import path from 'path';
import express from 'express';
import cookieParser from 'cookie-parser';
import cookieSession from 'cookie-session';
import passport from 'passport';
import oidc from 'openid-client';
import crypto from 'crypto';
import tenants from '../src/constants/tenants';
import PassportStrategyFactory from './passportStrategyFactory';
import { User, UserWriter, UserReader } from './user';

process.on('uncaughtException', function(err) {
  console.error('Uncaught Exception: ', err);
});

process.on('unhandledRejection', (reason, p) => {
  console.error('Unhandled Rejection: Promise:', p, 'Reason:', reason);
});

const strategyFactory = new PassportStrategyFactory(SERVER_URL);

function createUserProfile(client: oidc.Client, tokenSet: oidc.TokenSet, userInfo: oidc.UserinfoResponse, done: (err: any, user?: any) => void) {
  try {
    console.log(`OIDC verification phase`);
    if (typeof tokenSet === 'undefined' || typeof tokenSet.expires_in === 'undefined')
    {
      throw new Error('Invalid token-set.');
    }

    let user = new User(client, tokenSet, userInfo);
    done(null, user);
  }
  catch(err) {
    done(err, null);
  }
}

function createPassportStrategies(passport: passport.PassportStatic, tenants: string[], client: any) {
  tenants.forEach(tenant => {
    var openIdConnectStrategy = strategyFactory.getStrategy(
      tenant,
      client,
      (tokenSet: oidc.TokenSet, userInfo: oidc.UserinfoResponse, done: (err: any, user?: any) => void) =>
        createUserProfile(client, tokenSet, userInfo, done)
      );
    passport.use(`OpenIdConnect${tenant}`, openIdConnectStrategy);
  });
}

function createSignInEndpoint(expressApp: express.Express, passport: passport.PassportStatic, tenants: string[]) {
  tenants.forEach(tenant => {
    expressApp.get(
      `/auth/signin/${tenant.toLowerCase()}`,
      passport.authenticate(`OpenIdConnect${tenant}`));

    expressApp.post(
      `/auth/signin/callback-${tenant.toLowerCase()}`,
      passport.authenticate(`OpenIdConnect${tenant}`, {
        successRedirect: '/',
        failureRedirect: `/auth/signin/${tenant.toLowerCase()}`
      }),
      (req: express.Request, res: express.Response) => {
        if (req && res && req.session) {
          res.redirect(req.session.returnTo || '/');
        }
      });
  });
}

function createSignOutEndpoint(expressApp: express.Express) {
  expressApp.get(
    'auth/signout',
    (req: express.Request, _: express.Response) => {
      console.log('Processing signout request');
      req.logOut();
    }
  );
}

// Load environment variables from .env
const isSecure = (SERVER_PORT_HTTPS > 0 &&
  process.env.SSL_CERT_PFXFILE &&
  process.env.SSL_CERT_PASSPHRASE) ? true : false;

// Initialize Next.js
const nextApp = next({
  dir: '.',
  dev: DEV
});

// Add next-auth to next app
nextApp
  .prepare()
  .then(async () => {
    const issuer = await oidc.Issuer.discover(`${process.env.IDENTITY_URL}/.well-known/openid-configuration`);
    const client: any = new issuer.Client({
      client_id: 'ImAccessGraph',
      client_secret: 'secret'
    });

    console.log(`Preparing to start server [HTTP: ${SERVER_PORT_HTTP}, HTTPS: ${SERVER_PORT_HTTPS}]`);

    // If we have secure credentials for HTTPS then load cert and setup
    //  HTTP -> HTTPS redirection using second express instance
    if (SERVER_PORT_HTTPS > 0) {
      const unsecureApp = express();
      unsecureApp.use((req, res, next) => {
        if (req.secure) {
          next();
        } else {
          if (req.method === 'GET') {
            res.redirect(301, `https://${req.headers.host}:${SERVER_PORT_HTTPS}${req.originalUrl}`);
          } else {
            res.status(403).send(`Please use HTTPS over port ${SERVER_PORT_HTTPS} when submitting data to this server.`);
          }
        }
      });

      http
        .createServer(unsecureApp)
        .listen(SERVER_PORT_HTTP, () => {
          console.log(`> Ready on http://localhost:${SERVER_PORT_HTTP} [${DEV ? 'development' : 'production'}]`);
        });
    }

    const expressApp = express();
  
    // Revised session management
    const getHash: (payload: string) => string = (payload: string) => {
      const hmac = crypto.createHmac('sha256', 'sessionCookieSecret1234');
      hmac.update(payload);
      return hmac.digest('base64');
    };

    passport.serializeUser(
      (user: User, done: (err: any, cookieData: string) => void) => {
        const userData = UserWriter.writeTo(user);
        const userHash = getHash(userData);
        done(null, `${userData}:${userHash}`);
      });
    passport.deserializeUser(
      (cookieData: string, done: (err: any, user: User | null) => void) => {
        if (!cookieData || cookieData.length === 0) {
          done(null, null);
          return;
        }

        const parts = cookieData.split(':', 3);
        if(parts.length !== 2){
          done('invalid cookie', null);
          return;
        }

        const userData = parts[0];
        const userHash = parts[1];
        if (getHash(userData) !== userHash) {
          done('invalid cookie', null);
          return;
        }

        var user = UserReader.readFrom(client, userData);
        done(null, user);
      });

    // Static resources

    // Serve fonts from ionicons npm module
    expressApp.use('/fonts/ionicons', express.static('./node_modules/ionicons/dist/fonts'));
    
    // Setup express cookie-parser support
    expressApp.use(cookieParser(
      process.env.COOKIE_SECRET || ''
    ));

    // Setup express body-parser support
    expressApp.use(bodyParser.json());
    expressApp.use(bodyParser.urlencoded({ extended: true }));

    // Setup express session middleware 
    expressApp.use(cookieSession({
      name: 'idtymgr_session',
      secret: process.env.COOKIE_SECRET || '',
      domain: SERVER_HOST,
      path: '/',
      httpOnly: true,
      secure: isSecure,
      maxAge: 30 * 24 * 60 * 60 * 1000,
      //sameSite: 'lax'
    }));

    // Setup passport authentication and hook into express
    createPassportStrategies(passport, tenants, client);
    expressApp.use(passport.initialize());
    expressApp.use(passport.session());
    createSignInEndpoint(expressApp, passport, tenants);
    createSignOutEndpoint(expressApp);
    
    // Rather than require a GraphQL proxy, let us instead provide an endpoint that can be called
    //  to get an up-to-date access token/expiry date combo whenever needed. This will still keep
    //  the refresh token on the server side...
    expressApp.post(
      '/token',
      async (req: express.Request, res: express.Response) => {
        if (!req.user) {
          res.statusCode = 401;
          res.statusMessage = 'Unauthorized';
          res.end();
          return;
        }

        var user: User = <User>req.user;
        var result = await user.getAccessToken();
        res.statusCode = 200;
        res.contentType('application/json');
        res.write(JSON.stringify(result));
        res.end();
      }
    )

    // Catch-all handler to allow Next.js to handle all other routes
    expressApp.all('*', (req, res) => {
      const pathName = <string>url.parse(req.url, true).pathname;
      if (pathName === '/service-worker.js') {
        const filePath = path.join(__dirname, '..', '.next', pathName);
        return nextApp.serveStatic(req, res, filePath);
      } else {
        const nextRequestHandler = nextApp.getRequestHandler();
        return nextRequestHandler(req, res);
      }
    });

    // Start Next.js listener
    if (isSecure) {
      // Enable HSTS protocol
      expressApp.use(hsts({
        maxAge: 31536000,
        includeSubDomains: true,
        preload: false
      }));

      https
        .createServer({
          pfx: readFileSync(process.env.SSL_CERT_PFXFILE || ''),
          passphrase: process.env.SSL_CERT_PASSPHRASE  
        }, expressApp)
        .addListener('error', (error: Error) => {
          if (error) {
            throw error;
          }
        })
        .listen(SERVER_PORT_HTTPS, () => {
          console.log(`> Ready on https://localhost:${SERVER_PORT_HTTPS} [${DEV ? 'development' : 'production'}]`);
        });
    } else {
      expressApp
        .listen(SERVER_PORT_HTTP, (err: any) => {
          if (err) {
            throw err;
          }
          
          console.log(`> Ready on http://localhost:${SERVER_PORT_HTTP} [${DEV ? 'development' : 'production'}]`);
        });
    }
  })
  .catch(err => {
    console.log('An error occurred, unable to start the server');
    console.log(err);
  });
  