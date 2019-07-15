import dotenv from 'dotenv';
dotenv.config();

export const DEV = process.env.NODE_ENV !== 'production';
export const GA_TRACKING_ID = process.env.GA_TRACKING_ID;
export const GRAPHQL_ENDPOINT = process.env.GRAPHQL_URL;
export const SERVER_HOST = process.env.SERVER_HOST;
export const SERVER_PORT_HTTP = process.env.SERVER_PORT_HTTP ? parseInt(process.env.SERVER_PORT_HTTP) : 3000;
export const SERVER_PORT_HTTPS = process.env.SERVER_PORT_HTTPS ? parseInt(process.env.SERVER_PORT_HTTPS) : 0;
export const SERVER_URL = SERVER_PORT_HTTPS > 0 ? `https://${SERVER_HOST}:${SERVER_PORT_HTTPS}` : `https://${SERVER_HOST}:${SERVER_PORT_HTTP}`;
export const SITE_NAME = '';
export const SITE_TITLE = '';
export const SITE_DESCRIPTION = '';
export const SITE_IMAGE = '';
