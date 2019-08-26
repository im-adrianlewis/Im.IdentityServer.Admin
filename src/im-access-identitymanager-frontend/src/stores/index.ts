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

import { conversationSaga } from './conversation/sagas';
import { ConversationState } from './conversation/types';
import { ConversationReducer } from './conversation/reducer';

import { userQuerySaga } from './user-query/sagas';
import { UserQueryState } from './user-query/types';
import { UserQueryReducer } from './user-query/reducer';

export interface IApplicationState {
  //layout: LayoutState;
  route: RouterState;
  activeBrand: ActiveBrandState;
  analtyics: AnalyticsState;
  conversation: ConversationState;
  userQuery: UserQueryState;
}

export const rootReducer = combineReducers<IApplicationState>({
  //layout: LayoutReducer,
  route: routerReducer,
  activeBrand: ActiveBrandReducer,
  analtyics: AnalyticsReducer,
  conversation: ConversationReducer,
  userQuery: UserQueryReducer
});

export function* rootSaga() {
  yield all([
    fork(activeBrandSaga),
    fork(analyticsSaga),
    fork(conversationSaga),
    fork(userQuerySaga)
  ]);
}
