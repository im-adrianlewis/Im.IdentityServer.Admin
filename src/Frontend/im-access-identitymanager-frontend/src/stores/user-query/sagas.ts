import gql from 'graphql-tag';
import { AnyAction } from 'redux';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';
import { UserSearchCriteria, UserSearchVariables, UserSearchResponse, UserSelfResponse, UserQueryActionTypes } from './types';
import { fetchError, fetchSelfSuccess, fetchUsersSuccess } from './actions';
import client from '../linkLayer';
import { ApolloQueryResult } from 'apollo-client';
import GET_SELF_QUERY from './get-self.gql';
import GET_USERS_QUERY from './get-users.gql';

const getSelfQuery = () =>
  client.query<UserSelfResponse>({
    query: gql(GET_SELF_QUERY)
  });

const getUsersQuery = (criteria: UserSearchCriteria) =>
  client.query<UserSearchResponse, UserSearchVariables>({
    query: gql(GET_USERS_QUERY),
    variables: {
      criteria
    }
  });

function* handleFetchSelf(_: AnyAction) {
  try {
    const result: ApolloQueryResult<UserSelfResponse> =
      yield call(getSelfQuery);
      
    if (result.errors) {
      yield put(fetchError(result.errors.join(', ')));
    }
    else {
      yield put(fetchSelfSuccess(result.data.user));
    }
  }
  catch(error) {
    if (error instanceof Error) {
      yield put(fetchError(error.stack!));
    } else {
      yield put(fetchError('Unknown error occurred.'));
    }
  }
}

function* handleFetchUsers({payload: criteria}: AnyAction) {
  try {
    const result: ApolloQueryResult<UserSearchResponse> =
      yield call(getUsersQuery, criteria);
      
    if (result.errors) {
      yield put(fetchError(result.errors.join(', ')));
    }
    else {
      yield put(fetchUsersSuccess(result.data.pagedUsers));
    }
  }
  catch(error) {
    if (error instanceof Error) {
      yield put(fetchError(error.stack!));
    } else {
      yield put(fetchError('Unknown error occurred.'));
    }
  }
}

function* watchFetchSelfRequest() {
  yield takeEvery(UserQueryActionTypes.FETCH_SELF_REQUEST, handleFetchSelf);
}

function* watchFetchUsersRequest() {
  yield takeEvery(UserQueryActionTypes.FETCH_USERS_REQUEST, handleFetchUsers);
}

export function* userQuerySaga() {
  yield all([
    fork(watchFetchSelfRequest),
    fork(watchFetchUsersRequest)
  ]);
}