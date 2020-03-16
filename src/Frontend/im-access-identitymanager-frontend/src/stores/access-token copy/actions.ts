import { action } from 'typesafe-actions';
import { AccessTokenActionTypes, AccessToken } from './types';

export const fetchRequest = () =>
  action(AccessTokenActionTypes.FETCH_REQUEST);

export const fetchSuccess = (data: AccessToken) =>
  action(AccessTokenActionTypes.FETCH_SUCCESS, data);

export const fetchError = (message: string) =>
  action(AccessTokenActionTypes.FETCH_ERROR, message);
