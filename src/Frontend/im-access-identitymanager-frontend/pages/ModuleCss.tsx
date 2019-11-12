import * as React from 'react';
//import * as css from '../css/index.scss';
import {Layout} from '../src/components/Layout';

const ModuleCss: React.NextFunctionComponent<{}> = _ =>
  <Layout showNavMenu={true}>
    <div className="{css.home}">
      home
      <div>
        nested
      </div>
    </div>
  </Layout>;

ModuleCss.getInitialProps = async (_: any) => {
  return { };
};

export default ModuleCss;
