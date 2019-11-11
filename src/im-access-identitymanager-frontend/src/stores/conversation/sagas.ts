import gql from 'graphql-tag';
import { AnyAction } from 'redux';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';
import { ConversationResponse, ConversationVariables, ConversationActionTypes } from './types';
import { fetchError, fetchSuccess } from './actions';
import client from '../linkLayer';
import { ApolloQueryResult } from 'apollo-client';

import GET_CONVERSATION_QUERY from './get-conversation.gql';

const clientQuery = (conversationId: string) => 
  client.query<ConversationResponse, ConversationVariables>({
    query: gql(GET_CONVERSATION_QUERY),
    variables: {
      conversationId
    }
  });

function* handleFetch({payload: conversationId}: AnyAction) {
  try {
    const result: ApolloQueryResult<ConversationResponse> =
      yield call(clientQuery, conversationId);
      
    if (result.errors) {
      yield put(fetchError(result.errors.join(', ')));
    }
    else {
      yield put(fetchSuccess(result.data.conversation));
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
  yield takeEvery(ConversationActionTypes.FETCH_REQUEST, handleFetch);
}

export function* conversationSaga() {
  yield all([
    fork(watchFetchRequest)
  ]);
}