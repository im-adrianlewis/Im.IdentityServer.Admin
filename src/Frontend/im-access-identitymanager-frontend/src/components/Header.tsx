import * as React from 'react';
import { Container } from 'reactstrap';
import { MainMenu } from './MainMenu';
import css from '../../css/index.scss';

export const Header: React.FunctionComponent = _ =>
  <header>
    <Container cssModule={css} className={css['topbar']}>
      <MainMenu />
    </Container>
  </header>
;
