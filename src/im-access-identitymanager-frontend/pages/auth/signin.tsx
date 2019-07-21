import * as React from 'react';
import Router from 'next/router';
import { Layout } from '../../src/components/Layout';
//import { Container } from 'reactstrap';
import TenantDropdown from '../../src/components/TenantDropdown';

interface SignInProps {
}

class SignIn extends React.Component<SignInProps> {
  public render() {
    var boundSelectedTenant = this.onSelectedTenant.bind(this);

    return (
      <Layout showNavMenu={true}>
        <TenantDropdown onSelectedTenant={boundSelectedTenant} />
      </Layout>  
    );
  }

  private onSelectedTenant(tenant: string) {
    Router.push(`/auth/signin/${tenant.toLowerCase()}`);
  }
}

export default SignIn;