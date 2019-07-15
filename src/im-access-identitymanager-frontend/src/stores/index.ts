import { combineReducers/*, Dispatch, Reducer, Action, AnyAction*/ } from 'redux';
import { all, fork } from 'redux-saga/effects';
//import { routeReducer, RouterState } from 'react-router-redux';
//import { LayoutState, LayoutReducer } from './layout';

import { analyticsSaga } from './analytics/sagas';
import { AnalyticsState } from './analytics/types';
import { AnalyticsReducer } from './analytics/reducer';

import { conversationSaga } from './conversation/sagas';
import { ConversationState } from './conversation/types';
import { ConversationReducer } from './conversation/reducer';

export interface IApplicationState {
  //layout: LayoutState;
  conversation: ConversationState;
  analtyics: AnalyticsState;
}

export const rootReducer = combineReducers<IApplicationState>({
  //layout: LayoutReducer,
  conversation: ConversationReducer,
  analtyics: AnalyticsReducer
});

export function* rootSaga() {
  yield all([
    fork(conversationSaga),
    fork(analyticsSaga)
  ]);
}
