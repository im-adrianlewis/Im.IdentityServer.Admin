import * as React from 'react';
import { Layout } from './Layout';
import { NextPageContext } from 'next';

export class PageProps {
  //session: INextAuthSessionData;
  lang?: string;
}

export class Page<P = PageProps, S = {}, SS = any> extends React.Component<P, S, SS> {
  static async getInitialProps(_: NextPageContext) {
    return {
      //session: await NextAuth.init(context.req),
      lang: 'en'
    };
  }

  public adminAccessOnly() {
    return (
      <Layout {...this.props} showNavMenu={false}>
        <div className="text-center pt-5 pb-5">
          <h1 className="display-4 mb-5">Access Denied</h1>
          <p className="lead">You must be signed in as an administrator to access this page.</p>
        </div>
      </Layout>
    );
  }
}
