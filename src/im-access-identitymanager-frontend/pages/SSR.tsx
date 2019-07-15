import * as React from 'react';
import {Layout} from '../src/components/Layout';

const SSR: React.NextFunctionComponent<Props> = props =>
  <Layout showNavMenu={true}>
    {props.name}
  </Layout>;

SSR.getInitialProps = async (context) => {
  const props = {
    name: '"im-access-identitymanager-frontend" from client'
  };
  const server = !!context.req;

  if (server) {
    props.name = await mockFetchName();
  }

  return props;
};

export default SSR;

interface Props {
  name: string;
}

async function mockFetchName() {
  return '"im-access-identitymanager-frontend" from server';
}
