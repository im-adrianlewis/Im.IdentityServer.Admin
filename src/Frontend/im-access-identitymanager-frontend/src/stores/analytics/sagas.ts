import * as GA from 'react-ga';
import { GA_TRACKING_ID } from '../../constants/env';
import { DEV } from '../../constants/env';
import Router from 'next/router';
import * as qs from 'querystring';
import {addRouteCompleteEvent} from '../router';
import { AnyAction } from 'redux';
import { all, fork, put, takeEvery } from 'redux-saga/effects';
import { analyticsSuccess, analyticsSkipped } from './actions';
import { AnalyticsActionTypes } from './types';

function* handleAnalyticsRequest({payload: userId}: AnyAction) {
  try {
    if (GA_TRACKING_ID) {
      const gaOptions = userId ? {userId} : {};

      GA.initialize(GA_TRACKING_ID, {debug: DEV, gaOptions});
      GA.pageview([Router.pathname, qs.stringify(Router.query)].join('?'));

      addRouteCompleteEvent(url => GA.pageview(url));

      yield put(analyticsSuccess());
    }
    else {
      yield put(analyticsSkipped());
    }
  }
  catch(error) {
    yield put(analyticsSkipped());
  }
}

function* watchAnalyticsRequest() {
  yield takeEvery(AnalyticsActionTypes.ANALYTICS_REQUEST, handleAnalyticsRequest);
}

export function* analyticsSaga() {
  yield all([
    fork(watchAnalyticsRequest)
  ]);
}
