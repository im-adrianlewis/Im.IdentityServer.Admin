import next from 'next';
import { DEV, SERVER_HOST, SERVER_URL, SERVER_PORT_HTTP, SERVER_PORT_HTTPS } from '../src/constants/env';
import fs, { createReadStream } from 'fs';
import http from 'http';
import https from 'https';
import hsts from 'hsts';
import express from 'express';
import session from 'express-session';
import passport from 'passport';
import { Issuer, TokenSet, Strategy, VerifyCallback } from 'openid-client';
import crypto from 'crypto';

const tenants = [
  'Immediate',
  'RadioTimes',
  'GardenersWorld'
];

function createPassportStrategies(passport: passport.PassportStatic, tenants: string[], client: any) {
  tenants.forEach(tenant => {
    var openIdConnectStrategy =
      new Strategy({
        client: client,
        params: {
          client_id: 'ImAccessGraph',
          redirect_uri: `${SERVER_URL}/auth/signin/callback`,
          //response_type: 'code id_token token',
          response_type: 'code',
          response_mode: 'form_post',
          acr_values: 'tenant:RadioTimes',
          scope: 'openid profile',
          prompt: 'login'
        },
        passReqToCallback: false,
        sessionKey: 'idtymgr:oidc',
        usePKCE: false
      },
      (userInfo: any, tokenSet: TokenSet, verified: VerifyCallback) => {
        try {
          console.log(`OIDC verification phase`);

          let user = {
            id: userInfo.sub,
            displayName: userInfo.displayName,
            email: userInfo.email,
            emailVerified: userInfo.email_verified
          };

          let info = {
            accessToken: tokenSet.access_token,
            refreshToken: tokenSet.refresh_token,
            idToken: tokenSet.id_token
          };

          verified(null, user, info);
        }
        catch(err) {
          verified(err, null);
        }
      });

      passport.use(`OpenIdConnect${tenant}`, openIdConnectStrategy);
  });
}

function createSignInAuthenticate(expressApp: express.Express, passport: passport.PassportStatic, tenants: string[]) {
  tenants.forEach(tenant => {
    expressApp.get(
      `/auth/signin/${tenant.toLowerCase()}`,
      passport.authenticate(`OpenIdConnect${tenant}`));
  });
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

process.on('uncaughtException', function(err) {
  console.error('Uncaught Exception: ', err);
});

process.on('unhandledRejection', (reason, p) => {
  console.error('Unhandled Rejection: Promise:', p, 'Reason:', reason);
});

// Add next-auth to next app
nextApp
  .prepare()
  .then(async () => {
    const issuer = await Issuer.discover(`${process.env.IDENTITY_URL}/.well-known/openid-configuration`);
    const client = new issuer.Client();

    console.log(`Preparing to start server [HTTP: ${SERVER_PORT_HTTP}, HTTPS: ${SERVER_PORT_HTTPS}]`);

    // TODO: If we have secure credentials for HTTPS
    // then load cert and setup HTTP -> HTTPS redirection
    //  using second express instance
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
      (user, done: (err: any, cookieData: string) => void) => {
        const userData = new Buffer(JSON.stringify(user)).toString('base64');
        const userHash = getHash(userData);
        done(null, `${userData}:${userHash}`);
      });
    passport.deserializeUser(
      (cookieData: string, done: (err: any, user: any) => void) => {
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

        done(null, JSON.parse(new Buffer(userData, 'base64').toString()));
      });

    expressApp.use(session({
      cookie: {
        domain: SERVER_HOST,
        path: '/',
        httpOnly: true,
        secure: isSecure,
        maxAge: 30 * 24 * 60 * 60 * 1000,
        sameSite: true
      },
      name: '.idtymanager.auth',
      resave: true,
      saveUninitialized: true,
      secret: process.env.COOKIE_SECRET || ''
    }));

    createPassportStrategies(passport, tenants, client);
    expressApp.use(passport.initialize());
    expressApp.use(passport.session());
    createSignInAuthenticate(expressApp, passport, tenants);
    expressApp.get('/auth/signin/callback',
      passport.authenticate('OpenIdConnectImmediate', {failureRedirect: '/'}),
      (_/*req*/, res) => {
        res.redirect('/');
      });
    
    // Service worker
    expressApp.get('/sw.js', (_, res) => {
      res.setHeader('content-type', 'text/javascript');
      createReadStream('./offline/serviceWorker.js').pipe(res);
    });
  
    // Serve fonts from ionicons npm module
    expressApp.use('/fonts/ionicons', express.static('./node_modules/ionicons/dist/fonts'));
    
    // Catch-all handler to allow Next.js to handle all other routes
    expressApp.all('*', (req, res) => {
      const nextRequestHandler = nextApp.getRequestHandler();
      return nextRequestHandler(req, res);
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
          pfx: fs.readFileSync(process.env.SSL_CERT_PFXFILE || ''),
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
  