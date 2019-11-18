import { AnyAction } from 'redux';
import { all, /*call,*/ fork, put, takeEvery } from 'redux-saga/effects';
import { AccessTokenActionTypes } from './types';
import { fetchError, fetchSuccess } from './actions';
import moment from 'moment';

async function* handleFetch(/*{payload: token}: AnyAction*/) {
  try {
    var result = await fetch(
      '/token', {
        method: 'POST'
      });

    if (result.status !== 200) {
      yield put(fetchError(`Access token update failed: ${result.status}:${result.statusText}`));
    }
    else {
      var accessToken = await result.json();
      yield put(fetchSuccess({
        accessToken: accessToken.accessToken,
        refreshAfter: moment(accessToken.refreshAfter)
      }));
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
  yield takeEvery(AccessTokenActionTypes.FETCH_REQUEST, handleFetch);
}

export function* accessTokenSaga() {
  yield all([
    fork(watchFetchRequest)
  ]);
}
