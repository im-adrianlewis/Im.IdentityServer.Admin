import { compose } from 'recompose';
import makeStore from '../makeStore';
import withRedux from 'next-redux-wrapper';
import withReduxSaga from 'next-redux-saga';
import App from 'next/app';

const providers = compose<App, App>(
  withRedux(makeStore),
  withReduxSaga
);

export default providers;