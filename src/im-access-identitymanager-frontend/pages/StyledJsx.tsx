import * as React from 'react';
import {Layout} from '../src/components/Layout';

export default _ =>
  <Layout showNavMenu={true}>
    <div>
      {/*language=PostCSS*/}
      <style jsx> {`
          * {
              color: darkred;
          }
      `}
      </style>
      StyledJsx
    </div>
  </Layout>;
