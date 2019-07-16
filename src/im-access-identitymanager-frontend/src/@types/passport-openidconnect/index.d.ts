/// Type definitions for passport-openidconnect
/// <reference types="passport" />
/// <reference types="express" />

declare module 'passport-openidconnect' {
    import * as express from 'express';
    import * as passport from 'passport';

    function VerifyCallbackError(err: any);
    function VerifyCallbackUserProfile(err: any, user: any, info: any);
    export type VerifyCallback =
        VerifyCallbackError |
        VerifyCallbackUserProfile;

    function VerifyReqAction(req: express.Request, issuer: string, sub: string, verified: VerifyCallback);
    function VerifyReqProfileAction(req: express.Request, issuer: string, sub: string, profile: any, verified: VerifyCallback);
    function VerifyReqProfileTokensAction(req: express.Request, issuer: string, sub: string, profile: any, accessToken: string, refreshToken: string, verified: VerifyCallback);
    function VerifyReqProfileTokensParamsAction(req: express.Request, issuer: string, sub: string, profile: any, accessToken: string, refreshToken: string, params: any, verified: VerifyCallback);
    function VerifyReqProfileTokensParamsClaimsAction(req: express.Request, issuer: string, sub: string, profile: any, jwtClaims: any, accessToken: string, refreshToken: string, params: any, verified: VerifyCallback);
    export type VerifyReqFunction =
        VerifyReqAction | 
        VerifyReqProfileAction | 
        VerifyReqProfileTokensAction |
        VerifyReqProfileTokensParamsAction |
        VerifyReqProfileTokensParamsClaimsAction;

    function VerifyAction(issuer: string, sub: string, verified: VerifyCallback);
    function VerifyProfileAction(issuer: string, sub: string, profile: any, verified: VerifyCallback);
    function VerifyProfileTokensAction(issuer: string, sub: string, profile: any, accessToken: string, refreshToken: string, verified: VerifyCallback);
    function VerifyProfileTokensParamsAction(issuer: string, sub: string, profile: any, accessToken: string, refreshToken: string, params: any, verified: VerifyCallback);
    function VerifyProfileTokensParamsClaimsAction(issuer: string, sub: string, profile: any, jwtClaims: any, accessToken: string, refreshToken: string, params: any, verified: VerifyCallback);
    export type VerifyFunction =
        VerifyAction | 
        VerifyProfileAction | 
        VerifyProfileTokensAction |
        VerifyProfileTokensParamsAction |
        VerifyProfileTokensParamsClaimsAction;

    export function ErrorOrComplete<TResult>(err: any, result: TResult): void;

    export interface IAuthorityResolver {
        resolve(identifier: string, done: ErrorOrComplete<string>): void;
    }

    export type StateStore =
    StateStoreAction |
    StateStoreMetaAction;
    
    interface IStateVerify {
        verify(req: express.Request, state: string, done: (err: any, ok: boolean, state: any) => void);
    }

    export interface IStateStoreNoMeta extends IStateVerify {
        store(req: express.Request, done: ErrorOrComplete<string>): void;
    }

    export interface IStateStoreWithMeta extends IStateVerify {
        store(req: express.Request, meta: any, done: ErrorOrComplete<string>): void;
    }

    export type IStateStore =
        IStateStoreNoMeta |
        IStateStoreWithMeta;

    export interface OpenIdConnectStrategyOptions {
        identifierField: string | undefined;
        scope: string;
        callbackURL: string;
        passReqToCallback: boolean;
        skipUserProfile: boolean | undefined;
        sessionKey: string | undefined;
        authorizationURL: string | undefined;
        tokenURL: string | undefined;
        store: IStateStore;
        resolver: IAuthorityResolver | undefined;
        getClientCallback: (issuer: string, cb: ErrorOrResult<any>) => void;
    }

    export interface AuthorityConfiguration {
        callbackURL: string | undefined;
        clientID: string;
        max_age: number | undefined;
        ui_locals: string | undefined;
        id_token_hint: string | undefined;
        login_hint: string | undefined;
        acr_values: string | undefined;
        display: string | undefined;
        prompt: string | undefined;
        nonce: boolean | number | string | undefined;
        authorizationURL
    }

    export function ConfigureCallback(identifier: string, done: ErrorOrComplete<AuthorityConfiguration>): void;
    export class Strategy extends passport.Strategy {
        constructor(options: any, verify: VerifyFunction | VerifyReqFunction);
        authenticate(req: express.Request, options: any): any;
        configure(callback: ConfigureCallback): void;
        authorizationParams(options: any): any;
    }
}
