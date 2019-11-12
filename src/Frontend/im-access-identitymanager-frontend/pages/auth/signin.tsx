import * as React from 'react';
import { Form, Button, Card, CardTitle, CardBody, CardSubtitle, Row, Col, Container } from 'reactstrap';
import Router from 'next/router';
import { Layout } from '../../src/components/Layout';
import TenantDropdown from '../../src/components/TenantDropdown';
import css from '../../css/index.scss';

interface SignInProps {
}

interface SignInState {
  hasTenant: boolean;
  tenant: string;
}

class SignIn extends React.Component<SignInProps, SignInState> {
  constructor(props: Readonly<SignInProps>) {
    super(props);

    this.state = {
      hasTenant: false,
      tenant: ''
    };
  }

  public render() {
    var boundSelectedTenant = this.onSelectedTenant.bind(this);
    var boundNext = this.onNext.bind(this);

    return (
      <Layout showNavMenu={true}>
        <Card cssModule={css}>
          <Form cssModule={css}>
            <CardBody cssModule={css}>
              <CardTitle cssModule={css}>Brand</CardTitle>
              <CardSubtitle cssModule={css}>Select the brand then click next to sign-in.</CardSubtitle>
              <Container cssModule={css}>
                <Row cssModule={css}>
                  <Col cssModule={css} className="col-sm-10">
                    <TenantDropdown id="brand" onSelectedTenant={boundSelectedTenant} />
                  </Col>
                  <Col cssModule={css} className="col-sm-2">
                    <Button cssModule={css} disabled={!this.state.hasTenant} onClick={boundNext}>Next</Button>
                  </Col>
                </Row>
              </Container>
            </CardBody>
          </Form>
        </Card>
      </Layout>  
    );
  }

  private onNext(): void {
    if (this.state.hasTenant) {
      Router.push(`/auth/signin/${this.state.tenant.toLowerCase()}`);
    }
  }

  private onSelectedTenant(tenant: string): void {
    this.setState({
      hasTenant: true,
      tenant: tenant
    });
  }
}

export default SignIn;