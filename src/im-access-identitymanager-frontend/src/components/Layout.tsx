import * as React from 'react';
import {Header} from './Header';
import {Footer} from './Footer';
import Head from 'next/head';
//import css from '../../css/index.scss';

interface LayoutProps {
  showNavMenu: boolean;
}

export class Layout extends React.Component<LayoutProps> {

  public render() {
    return (
      <>
        <Head>
          <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
          {/* <style dangerouslySetInnerHTML={{__html: css}} /> */}
          <link rel="shortcut icon" href="/static/favicon.ico" />
          <link rel="manifest" href="/static/manifest.json" />
          <script src="https://cdn.polyfill.io/v2/polyfill.min.js?features=default,Array.prototype.find,Array.prototype.includes,String.prototype.includes,Array.prototype.findIndex,Object.entries"></script>
        </Head>
        <Header />
        <main>
          {this.props.children}
        </main>
        <Footer />
      </>
    );
  }
}
