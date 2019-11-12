import * as React from 'react';
import { Container } from 'reactstrap';
import css from '../../css/index.scss';

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
