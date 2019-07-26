import { combineReducers/*, Dispatch, Reducer, Action, AnyAction*/ } from 'redux';
import { all, fork } from 'redux-saga/effects';

// import { LayoutState, LayoutReducer } from './layout';

// import { RouterState } from 'connected-next-router/types';
// import { routerReducer } from 'connected-next-router/es';

import { analyticsSaga } from './analytics/sagas';
import { AnalyticsState } from './analytics/types';
import { AnalyticsReducer } from './analytics/reducer';

import { activeBrandSaga } from './active-brand/sagas';
import { ActiveBrandState } from './active-brand/types';
import { ActiveBrandReducer } from './active-brand/reducer';

import { conversationSaga } from './conversation/sagas';
import { ConversationState } from './conversation/types';
import { ConversationReducer } from './conversation/reducer';

export interface IApplicationState {
  //layout: LayoutState;
  //route: RouterState;
  activeBrand: ActiveBrandState;
  conversation: ConversationState;
  analtyics: AnalyticsState;
}

export const rootReducer = combineReducers<IApplicationState>({
  //layout: LayoutReducer,
  //route: routerReducer,
  activeBrand: ActiveBrandReducer,
  conversation: ConversationReducer,
  analtyics: AnalyticsReducer
});

export function* rootSaga() {
  yield all([
    fork(activeBrandSaga),
    fork(conversationSaga),
    fork(analyticsSaga)
  ]);
}
