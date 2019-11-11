import * as React from 'react';
import css from '../../css/index.scss';
import classNames from 'classnames';
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
        <div className={classNames(css['text-center'], css['pt-5'], css['pb-5'])}>
          <h1 className={classNames(css['display-4'], css['mb-5'])}>Access Denied</h1>
          <p className={classNames(css['lead'])}>You must be signed in as an administrator to access this page.</p>
        </div>
      </Layout>
    );
  }
}
