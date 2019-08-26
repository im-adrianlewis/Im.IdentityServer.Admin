import { Store, Action, createStore, applyMiddleware } from 'redux';
import createSagaMiddleware, { Task } from 'redux-saga';
import { createRouterMiddleware, initialRouterState } from 'connected-next-router';
import { composeWithDevTools } from 'redux-devtools-extension';
import { createLogger } from 'redux-logger';
import { MakeStoreOptions } from 'next-redux-wrapper';
import { IApplicationState, rootReducer, rootSaga } from './stores';

interface SagaStore extends Store<IApplicationState, Action<any>> {
  sagaTask: Task;
}

export default function makeStore(
  initialState: IApplicationState,
  options: MakeStoreOptions
): Store<IApplicationState> {
  if (options.isServer && typeof window === 'undefined') {
    // Server-side
    if (initialState && !initialState.route) {
      initialState.route = initialRouterState('/');
    }

    const sagaMiddleware = createSagaMiddleware();
    const routerMiddleware = createRouterMiddleware();

    const store = createStore<IApplicationState, Action<any>, {}, {}>(
      rootReducer,
      initialState,
      applyMiddleware(
        sagaMiddleware,
        routerMiddleware)
    ) as SagaStore;
    
    store.sagaTask = sagaMiddleware.run(rootSaga);

    return store;
  } else {
    // Client-side
    const composeEnhancers = composeWithDevTools({});
    const sagaMiddleware = createSagaMiddleware();
    const routerMiddleware = createRouterMiddleware();
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
          routerMiddleware,
          loggingMiddleware))
    ) as SagaStore;

    store.sagaTask = sagaMiddleware.run(rootSaga);

    return store;
  } 
}
