/// Type definitions for hsts
/// <reference types="express" />

declare module 'hsts' {

    import { RequestHandler } from 'express';

    export interface HstsOptions {
        maxAge: number;
        includeSubDomains: boolean;
        preload: boolean;
    }

    export default function hsts(options: HstsOptions): RequestHandler;
}
