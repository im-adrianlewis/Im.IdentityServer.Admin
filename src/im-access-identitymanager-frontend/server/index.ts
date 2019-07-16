import next from 'next';
import dotenv from 'dotenv';
import { DEV, SERVER_HOST, SERVER_URL, SERVER_PORT_HTTP, SERVER_PORT_HTTPS } from '../src/constants/env';
import fs, { createReadStream } from 'fs';
import http from 'http';
import https from 'https';
import hsts from 'hsts';
import express from 'express';
import session from 'express-session';
import passport from 'passport';
import { Strategy, VerifyCallback, Client } from 'passport-openidconnect';
import crypto from 'crypto';

// Load environment variables from .env
dotenv.config();
const isSecure = (SERVER_PORT_HTTPS > 0 &&
  process.env.SSL_CERT_PFXFILE &&
  process.env.SSL_CERT_PASSPHRASE) ? true : false;

class AuthorityResolver {
  constructor(issuer: string) {
    this._issuer = issuer;
  }

  public resolve(identifier: string, done: (err: any, issuer: string) => void) {
    console.log(`AuthorityResolver: [${identifier}]`);
    done(null, this._issuer);
  }

  private _issuer: string;
}

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
  .then(() => {
    var openIdConnectStrategy =
      new Strategy({
        identifierField: 'name_identifier',
        passReqToCallback: true,
        scope: 'profile',
        skipUserProfile: false,
        sessionKey: 'idtymgr:oidc',
        resolver: new AuthorityResolver(process.env.IDENTITY_URL || ''),
        getClientCallback: (iss: string, cb: (err: any, client: Client) => void) => {
          console.log(`Building OpenIdConnect client configuration for [${iss}]`);
          cb(null, {
            id: process.env.IDENTITY_CLIENT_ID || '',
            secret: process.env.IDENTITY_CLIENT_SECRET || '',
            redirectURIs: [`${SERVER_URL}/auth/signin/callback`]
          });
        } 
      },
      (req: express.Request, issuer: string, sub: string, profile: any, jwtClaims: any, accessToken: string, refreshToken: string, params: any, verified: VerifyCallback) => {
        try {
          console.log(`OIDC verification: [${req}, ${issuer}, ${params}]`);

          let user = {
            id: sub,
            displayName: profile.displayName,
            email: profile._raw.email,
            emailVerified: profile._raw.email_verified
          };

          const now = Date.now();

          let info = {
            accessToken,
            refreshToken,
            expiryAfter: jwtClaims.exp,
            refreshAfter: now + (jwtClaims.exp - now) / 2
          };

          verified(null, user, info);
        }
        catch(err) {
          verified(err);
        }
      });

    console.log(`Preparing to start server [HTTP: ${SERVER_PORT_HTTP}, HTTPS: ${SERVER_PORT_HTTPS}]`);

    // TODO: If we have secure credentials for HTTPS
    // then load cert and setup HTTP -> HTTPS redirection
    //  using second express instance
    if (SERVER_PORT_HTTPS > 0) {
      //const unsecureExpress = require('express');
      //const unsecureApp = unsecureExpress();
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

    passport.use(openIdConnectStrategy);
    expressApp.use(passport.initialize());
    expressApp.use(passport.session());
    expressApp.get('/auth/signin', passport.authenticate('openidconnect'));
    expressApp.get('/auth/signin/callback',
      passport.authenticate('openidconnect', {failureRedirect: '/'}),
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
        .listen(SERVER_PORT_HTTPS, (err: any) => {
          if (err) {
            throw err;
          }
          
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
  