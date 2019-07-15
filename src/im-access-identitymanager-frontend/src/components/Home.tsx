import * as React from 'react';
import classNames from 'classnames';
//import * as css from '../../css/index.scss';

export const Home: React.FunctionComponent = _ =>
  <div className={classNames('test', 'css.home')}>
    <ul>
      <li>
        usage classnames in Home.tsx
      </li>
      <li>
        Layout.tsx set background-color hot-pink using global styled jsx
      </li>
    </ul>
  </div>;
