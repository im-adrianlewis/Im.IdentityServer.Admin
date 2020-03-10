import * as React from 'react';
import {Header} from './Header';
import {Main} from './Main';
import {Footer} from './Footer';
import Head from 'next/head';

interface LayoutProps {
  showNavMenu: boolean;
}

export class Layout extends React.Component<LayoutProps> {

  public render() {
    return (
      <>
        <Head>
          <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
          <link rel="shortcut icon" href="/static/favicon.ico" />
          <link rel="manifest" href="/static/manifest.json" />
          <script src="https://cdn.polyfill.io/v2/polyfill.min.js?features=default,Array.prototype.find,Array.prototype.includes,String.prototype.includes,Array.prototype.findIndex,Object.entries"></script>
        </Head>
        <Header />
        <Main>
          {this.props.children}
        </Main>
        <Footer />
      </>
    );
  }
}
