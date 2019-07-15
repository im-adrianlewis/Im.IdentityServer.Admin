import 'isomorphic-fetch';
import * as React from 'react';
import { Provider } from 'react-redux';
import App, { Container/*, AppContext*/ } from 'next/app';
import makeStore from '../src/makeStore';
import withRedux from 'next-redux-wrapper';
import NextSeo from 'next-seo';
import SEO from '../next-seo.config';

interface IdentityManagerAppProps {
  store?: any;
}

class IdentityManagerApp extends App<IdentityManagerAppProps> {
  /*static async getInitialProps(appContext: AppContext) {
    let pageProps = {};

    if (appContext.Component.getInitialProps) {
      pageProps = await appContext.Component.getInitialProps(appContext.ctx);
    }

    return { pageProps };
  }*/

  public render() {
    const { Component, pageProps, store } = this.props;

    return (
      <Container>
        <Provider store={store}>
          <NextSeo config={SEO} />
          <Component {...pageProps} />
        </Provider>
      </Container>
    );
  }
}

export default withRedux(makeStore)(IdentityManagerApp);
