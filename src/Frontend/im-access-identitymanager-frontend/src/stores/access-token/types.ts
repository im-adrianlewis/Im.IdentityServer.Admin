import moment from "moment";

export interface AccessToken {
  accessToken: string;
  refreshAfter: moment.Moment;
}

export interface AccessTokenRequest {
}

export interface AccessTokenResponse {
  token: AccessToken;
}

export enum AccessTokenActionTypes {
  FETCH_REQUEST = '@@AccessToken/FETCH_REQUEST',
  FETCH_SUCCESS = '@@AccessToken/FETCH_SUCCESS',
  FETCH_ERROR = '@@AccessToken/FETCH_ERROR'
}

export interface AccessTokenState {
  readonly id?: string;
  readonly loading: boolean;
  readonly data?: AccessToken;
  readonly error?: string;
}
