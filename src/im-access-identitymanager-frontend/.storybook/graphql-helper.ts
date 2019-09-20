import { setupGraphiQL } from '@storybook/addon-graphql';
import { GRAPHQL_ENDPOINT } from '../src/constants/env';

const fetcher = (params: any) => {
  // TODO: For iGraphQL to work correctly we might need to add an auth header
  const options = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(params)
  };

  return fetch(GRAPHQL_ENDPOINT, options).then(res => res.json());
};

const graphiQl = setupGraphiQL({ 
  url: GRAPHQL_ENDPOINT,
  fetcher
});

export default graphiQl;
