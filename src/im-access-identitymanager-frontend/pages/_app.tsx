import 'isomorphic-fetch';
import * as React from 'react';
import { Provider } from 'react-redux';
import App, { AppContext } from 'next/app';
import makeStore from '../src/makeStore';
import withRedux from 'next-redux-wrapper';
import withReduxSaga from 'next-redux-saga';

//import { HelmetProvider } from 'react-helmet-async';
// import NextSeo from 'next-seo';
// import SEO from '../next-seo.config';

export interface IdentityManagerAppProps {
  store?: any;
}

export class IdentityManagerApp extends App<IdentityManagerAppProps> {
  static async getInitialProps(appContext: AppContext) {
    let pageProps = {};
    //let worker = new Worker();

    if (appContext.Component.getInitialProps) {
      pageProps = await appContext.Component.getInitialProps(appContext.ctx);
    }

    return { pageProps };
  }

  //public helmetContext = {};

  public render() {
    const { Component, pageProps, store } = this.props;

    return (
      <Provider store={store}>
        {/* <HelmetProvider context={this.helmetContext}> */}
          {/* <NextSeo /> */}
          {/* <Component {...pageProps} helmetContext={this.helmetContext} /> */}
          <Component {...pageProps} />
        {/* </HelmetProvider> */}
      </Provider>
    );
  }
}

export default withRedux(makeStore)(withReduxSaga(IdentityManagerApp));
