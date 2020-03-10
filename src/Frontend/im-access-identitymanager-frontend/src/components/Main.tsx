import * as React from 'react';
import { Container } from 'reactstrap';

export class Main extends React.Component {
  public render() {
    return (
      <main>
        <Container>
          {this.props.children}
        </Container>
      </main>
    );
  }
}
