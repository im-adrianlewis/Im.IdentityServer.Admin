import { AnyAction } from 'redux';
import { all, /*call,*/ fork, put, takeEvery } from 'redux-saga/effects';
import { /*ActiveBrandResponse, ActiveBrandVariables,*/ ActiveBrandActionTypes } from './types';
import { /*fetchError,**/ fetchSuccess } from './actions';

function* handleFetch({payload: activeBrand}: AnyAction) {
  /*try {
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
  }*/
  console.log(activeBrand);
  yield put(fetchSuccess({
    availableBrands: [],
    selectedBrand: ''
  }));
}

function* handleUpdate({payload: activeBrand}: AnyAction) {
  /*try {
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
  }*/
  console.log(activeBrand);
  yield put(fetchSuccess({
    availableBrands: [],
    selectedBrand: ''
  }));
}

function* watchFetchRequest() {
  yield takeEvery(ActiveBrandActionTypes.FETCH_REQUEST, handleFetch);
}

function* watchUpdateRequest() {
  yield takeEvery(ActiveBrandActionTypes.UPDATE_BRAND, handleUpdate);
}

export function* activeBrandSaga() {
  yield all([
    fork(watchFetchRequest),
    fork(watchUpdateRequest)
  ]);
}