import { combineReducers/*, Dispatch, Reducer, Action, AnyAction*/ } from 'redux';
import { all, fork } from 'redux-saga/effects';

// import { LayoutState, LayoutReducer } from './layout';

import { RouterState } from 'connected-next-router/types';
import { routerReducer } from 'connected-next-router';

import { analyticsSaga } from './analytics/sagas';
import { AnalyticsState } from './analytics/types';
import { AnalyticsReducer } from './analytics/reducer';

import { activeBrandSaga } from './active-brand/sagas';
import { ActiveBrandState } from './active-brand/types';
import { ActiveBrandReducer } from './active-brand/reducer';

import { userQuerySaga } from './user-query/sagas';
import { UserQueryState } from './user-query/types';
import { UserQueryReducer } from './user-query/reducer';

import { circuitBreakerPolicySaga } from './circuit-breaker-policies/sagas';
import { CircuitBreakerPolicyState } from './circuit-breaker-policies/types';
import { CircuitBreakerPolicyReducer } from './circuit-breaker-policies/reducer';

export interface IApplicationState {
  //layout: LayoutState;
  route: RouterState;
  activeBrand: ActiveBrandState;
  analtyics: AnalyticsState;
  userQuery: UserQueryState;
  circuitBreakerPolicies: CircuitBreakerPolicyState;
}

export const rootReducer = combineReducers<IApplicationState>({
  //layout: LayoutReducer,
  route: routerReducer,
  activeBrand: ActiveBrandReducer,
  analtyics: AnalyticsReducer,
  userQuery: UserQueryReducer,
  circuitBreakerPolicies: CircuitBreakerPolicyReducer
});

export function* rootSaga() {
  yield all([
    fork(activeBrandSaga),
    fork(analyticsSaga),
    fork(userQuerySaga),
    fork(circuitBreakerPolicySaga)
  ]);
}
