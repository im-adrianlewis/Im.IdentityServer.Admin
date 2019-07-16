import * as React from 'react';
import Document, { Head, Main, NextScript, DocumentContext } from 'next/document';

export default class extends Document {
  static async getInitialProps(context: DocumentContext) {
    const documentProps = await Document.getInitialProps(context);
    const page = context.renderPage();

    return {...documentProps, ...page};
  }

  public render() {
    return (
      <html lang={this.props.__NEXT_DATA__.props.pageProps && this.props.__NEXT_DATA__.props.pageProps.lang || 'en'}>
        <Head>
        </Head>
        <body>
          <Main />
          <NextScript />
        </body>
      </html>
    );
  }
}
