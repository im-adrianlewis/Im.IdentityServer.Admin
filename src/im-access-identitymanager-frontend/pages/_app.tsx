import 'isomorphic-fetch';
import * as React from 'react';
import { Provider } from 'react-redux';
import App, { Container, AppContext } from 'next/app';
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

    if (appContext.Component.getInitialProps) {
      pageProps = await appContext.Component.getInitialProps(appContext.ctx);
    }

    return { pageProps };
  }

  //public helmetContext = {};

  public render() {
    const { Component, pageProps, store } = this.props;

    return (
      <Container>
        <Provider store={store}>
          {/* <HelmetProvider context={this.helmetContext}> */}
            {/* <NextSeo /> */}
            {/* <Component {...pageProps} helmetContext={this.helmetContext} /> */}
            <Component {...pageProps} />
          {/* </HelmetProvider> */}
        </Provider>
      </Container>
    );
  }
}

export default withRedux(makeStore)(withReduxSaga(IdentityManagerApp));
