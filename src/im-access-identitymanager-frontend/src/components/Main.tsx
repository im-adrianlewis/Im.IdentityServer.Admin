import * as React from 'react';
import css from '../../css/index.scss';
import { Container } from 'reactstrap';

export class Main extends React.Component {
  public render() {
    return (
      <main>
        <Container cssModule={css}>
          {this.props.children}
        </Container>
      </main>
    );
  }
}
