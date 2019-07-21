/// Type definitions for passport-openidconnect
/// <reference types="passport" />
/// <reference types="express" />
/// <reference types="openid-client" />

declare module 'openid-client' {
    import { Request } from "express";
    import * as passport from 'passport';
    import { IncomingMessage } from 'http';
    import { GotOptions } from 'got';
    
    export { }; // Disable automatic export of all module members (make it explicit)
    
    //
    
    type HttpRequestOptions = GotOptions<null>;
    type CustomHttpOptionsProvider = (options: HttpRequestOptions) => HttpRequestOptions;
    
    export const custom: {
        readonly http_options: unique symbol,
    };
    
    // https://github.com/panva/node-openid-client/tree/master/docs#issuer
    
    export interface IssuerMetadata {
        readonly issuer?: string;
        readonly authorization_endpoint?: string;
        readonly userinfo_endpoint?: string;
        readonly token_endpoint?: string;
        readonly jwks_uri?: string;
        readonly revocation_endpoint?: string;
        readonly introspection_endpoint?: string;
        readonly end_session_endpoint?: string;
        readonly registration_endpoint?: string;
        readonly token_endpoint_auth_methods_supported?: ReadonlyArray<string>;
        readonly token_endpoint_auth_signing_alg_values_supported?: ReadonlyArray<string>;
        readonly introspection_endpoint_auth_methods_supported?: ReadonlyArray<string>;
        readonly introspection_endpoint_auth_signing_alg_values_supported?: ReadonlyArray<string>;
        readonly revocation_endpoint_auth_methods_supported?: ReadonlyArray<string>;
        readonly revocation_endpoint_auth_signing_alg_values_supported?: ReadonlyArray<string>;
        readonly mtls_endpoint_aliases?: {
            token_endpoint?: string;
            userinfo_endpoint?: string;
            revocation_endpoint?: string;
            introspection_endpoint?: string;
        };
    }
    
    export class Issuer {
        static [custom.http_options]: CustomHttpOptionsProvider;
    
        constructor(metadata?: IssuerMetadata);
    
        static discover(issuer: string): Promise<Issuer>;
    
        [custom.http_options]: CustomHttpOptionsProvider;
    
        readonly metadata: IssuerMetadata;
    
        readonly Client: typeof Client;
    
        keystore(forceReload?: boolean): Promise<unknown>;
    }
    
    export interface ClientMetadata {
        readonly client_id?: string;
        readonly client_secret?: string;
        readonly redirect_uris?: ReadonlyArray<string>;
        readonly response_types?: ReadonlyArray<string>;
        readonly post_logout_redirect_uris?: ReadonlyArray<string>;
    }
    
    export interface AuthorizationUrlParameters {
        readonly redirect_uri?: string;
        readonly response_type?: string;
        readonly scope?: string;
    
        readonly response_mode?: string;
        readonly nonce?: string;
        readonly resource?: string;
        readonly code_challenge?: string;
        readonly code_challenge_method?: string;
    }
    
    export interface EndSessionUrlParameters {
        readonly id_token_hint?: string | TokenSet;
        readonly post_logout_redirect_uri?: string;
        readonly state?: string;
    }
    
    export class Client {
        static [custom.http_options]: CustomHttpOptionsProvider;
    
        constructor(metadata?: ClientMetadata);
    
        [custom.http_options]: CustomHttpOptionsProvider;
    
        readonly metadata: ClientMetadata;
    
        authorizationUrl(parameters?: AuthorizationUrlParameters): string;
    
        endSessionUrl(parameters?: EndSessionUrlParameters): string;
    
        callbackParams(input: string | IncomingMessage): {};
    
        callback(
            redirectUri: string,
            parameters: {},
            checks?: {
                readonly response_type?: string;
                readonly state?: string;
                readonly nonce?: string;
                readonly code_verifier?: string;
                readonly max_age?: number;
            }
        ): Promise<TokenSet>;
    
        userinfo(accessToken: string | TokenSet): Promise<{ readonly [name: string]: {} | null | undefined }>;
    
        grant(body: {
            readonly grant_type: 'authorization_code' | 'client_credentials' | 'password' | 'refresh_token' | string;
            readonly [name: string]: string | undefined;
        }): Promise<TokenSet>;
    
        introspect(
            token: string,
            tokenTypeHint?: string,
            extras?: { readonly introspectBody?: object }
        ): Promise<{ readonly [name: string]: {} | null | undefined }>;
    }
    
    export class TokenSet {
        readonly access_token?: string;
        readonly token_type?: string;
        readonly id_token?: string;
        readonly refresh_token?: string;
        readonly expires_in?: number;
    
        expired(): boolean;
    
        claims(): { readonly [name: string]: {} | null | undefined };
    }
    
    export namespace generators {
        // https://github.com/panva/node-openid-client/tree/master/docs#generators
    
        function random(bytes?: number): string;
    
        function state(bytes?: number): string;
    
        function nonce(bytes?: number): string;
    
        function codeVerifier(bytes?: number): string;
    
        function codeChallenge(verifier: string): string;
    }
        
    export interface IStrategyOptions {
        client: Client;
        params: any;
        passReqToCallback: boolean;
        sessionKey: string;
        usePKCE: boolean;
    }

    type VerifyCallbackError = (err: any) => void;
    type VerifyCallbackSuccess = (err: any, user: any, info?: any) => void;
    export type VerifyCallback = VerifyCallbackError | VerifyCallbackSuccess;

    type VerifyReqAction = (req: Request, tokenSet: TokenSet, verified: VerifyCallback) => void;
    type VerifyReqProfileAction = (req: Request, userInfo: any, tokenSet: TokenSet, verified: VerifyCallback) => void;
    export type VerifyReqFunction = VerifyReqAction | VerifyReqProfileAction;

    type VerifyAction = (tokenSet: TokenSet, verified: VerifyCallback) => void;
    type VerifyProfileAction = (userInfo: any, tokenSet: TokenSet, verified: VerifyCallback) => void;
    export type VerifyFunction = VerifyAction | VerifyProfileAction;

    export class Strategy extends passport.Strategy {
        constructor(options: IStrategyOptions, verify: VerifyFunction | VerifyReqFunction);

        authenticate(req: Request, options: any);
    }
}