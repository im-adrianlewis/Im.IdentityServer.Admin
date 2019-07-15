import { Store, Action, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';
//import { connectRouter, routerMiddleware } from 'connected-react-router';
import { composeWithDevTools } from 'redux-devtools-extension';
import { createLogger } from 'redux-logger';
import {  } from 'next-redux-wrapper';
//import { History } from 'history';
import { IApplicationState, rootReducer, rootSaga } from './stores';
//import { sessionRequest } from './stores/system/actions';
import { IncomingMessage, ServerResponse } from 'http';

interface IStoreOptions {
  isServer: boolean;
  req?: IncomingMessage;
  res?: ServerResponse;
  query?: any;
}

interface SagaStore extends Store<IApplicationState, Action<any>> {
  sagaTask: any;
}

export default function makeStore(
  initialState: IApplicationState,
  options: IStoreOptions
): Store<IApplicationState> {
  if (options.isServer && typeof window === 'undefined') {
    const sagaMiddleware = createSagaMiddleware();
    const store = createStore<IApplicationState, Action<any>, {}, {}>(
      rootReducer,
      initialState,
      applyMiddleware(sagaMiddleware)
    ) as SagaStore;
    
    store.sagaTask = sagaMiddleware.run(rootSaga);

    return store;
  } else {
    const composeEnhancers = composeWithDevTools({});
    const sagaMiddleware = createSagaMiddleware();
    const loggingMiddleware = createLogger({
      predicate: (_/*getState*/, action) => !/^@@/.test(action.type),
      collapsed: true
    });

    const store = createStore<IApplicationState, Action<any>, {}, {}>(
      rootReducer,
      initialState,
      composeEnhancers(
        applyMiddleware(
          sagaMiddleware,
          loggingMiddleware))
    ) as SagaStore;

    store.sagaTask = sagaMiddleware.run(rootSaga);

    return store;
  } 
}
