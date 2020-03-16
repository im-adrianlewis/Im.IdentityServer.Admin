import gql from 'graphql-tag';
import { AnyAction } from 'redux';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';
import { CircuitBreakerPolicyResponse, CircuitBreakerPolicyActionTypes } from './types';
import { fetchError, fetchSuccess } from './actions';
import client from '../linkLayer';
import { ApolloQueryResult } from 'apollo-client';

import QUERY_CIRCUITBREAKERPOLICIES_QUERY from './query-circuit-breaker-policies.gql';
//import SUBSCRIBE_CIRCUITBREAKERPOLICIES_QUERY from './subscribe-circuit-breaker-policies.gql';

const clientQuery = () => 
  client.query<CircuitBreakerPolicyResponse>({
    query: gql(QUERY_CIRCUITBREAKERPOLICIES_QUERY)
  });

// const clientSubscribe = () => 
//   client.subscribe<CircuitBreakerPolicyResponse>({
//     query: gql(SUBSCRIBE_CIRCUITBREAKERPOLICIES_QUERY)
//   });

function* handleFetch({}: AnyAction) {
  try {
    const result: ApolloQueryResult<CircuitBreakerPolicyResponse> =
      yield call(clientQuery);
      
    if (result.errors) {
      yield put(fetchError(result.errors.join(', ')));
    }
    else {
      yield put(fetchSuccess(result.data.policies));
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

function* watchFetchRequest() {
  yield takeEvery(CircuitBreakerPolicyActionTypes.FETCH_REQUEST, handleFetch);
}

export function* circuitBreakerPolicySaga() {
  yield all([
    fork(watchFetchRequest)
  ]);
}