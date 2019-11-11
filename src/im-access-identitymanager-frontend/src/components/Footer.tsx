import * as React from 'react';
import { Container } from 'reactstrap';
import css from '../../css/index.scss';

export const Footer: React.FunctionComponent = _ =>
  <footer>
    <Container cssModule={css}>
      <h2 className={css.h2}>© Immediate Media 2019</h2>
    </Container>
  </footer>;
