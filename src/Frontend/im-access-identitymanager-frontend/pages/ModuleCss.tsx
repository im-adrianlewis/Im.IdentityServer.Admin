import * as React from 'react';
import {Layout} from '../src/components/Layout';

const ModuleCss: React.NextFunctionComponent<{}> = _ =>
  <Layout showNavMenu={true}>
    <div className="home">
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
