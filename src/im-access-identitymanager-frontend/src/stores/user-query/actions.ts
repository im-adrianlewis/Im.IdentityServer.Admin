import { action } from 'typesafe-actions';
import { UserItem, UserQueryActionTypes, UserSearchCriteria, PagedUsers } from './types';

export const fetchSelfRequest = () =>
  action(UserQueryActionTypes.FETCH_SELF_REQUEST);

export const fetchSelfSuccess = (data: UserItem) =>
  action(UserQueryActionTypes.FETCH_SELF_SUCCESS, data);

export const fetchUsersRequest = (criteria: UserSearchCriteria) =>
  action(UserQueryActionTypes.FETCH_USERS_REQUEST, criteria);

export const fetchUsersSuccess = (data: PagedUsers) =>
  action(UserQueryActionTypes.FETCH_USERS_SUCCESS, data);

export const fetchError = (message: string) =>
  action(UserQueryActionTypes.FETCH_ERROR, message);
