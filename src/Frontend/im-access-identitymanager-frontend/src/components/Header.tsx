import * as React from 'react';
import { Container } from 'reactstrap';
import { MainMenu } from './MainMenu';

export const Header: React.FunctionComponent = _ =>
  <header>
    <Container className={'topbar'}>
      <MainMenu />
    </Container>
  </header>
;
