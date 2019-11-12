import fetch from 'isomorphic-fetch';
import { GRAPHQL_REGULAR_ENDPOINT, GRAPHQL_SUBSCRIPTION_ENDPOINT } from '..//constants/env';
import { split } from 'apollo-link';
import { HttpLink } from 'apollo-link-http';
import { WebSocketLink } from 'apollo-link-ws';
import { getMainDefinition } from 'apollo-utilities';
import { ApolloClient, InMemoryCache } from 'apollo-boost';

const httpLink = new HttpLink({
  uri: GRAPHQL_REGULAR_ENDPOINT,
  fetch
});

const wsLink = new WebSocketLink({
  uri: GRAPHQL_SUBSCRIPTION_ENDPOINT ?? '',
  options: {
    reconnect: true
  }
});

const link = split(
  ({ query }) => {
    const definition = getMainDefinition(query);
    return (
      definition.kind === 'OperationDefinition' &&
      definition.operation === 'subscription'
    );
  },
  wsLink,
  httpLink
);

const client = new ApolloClient({
  link: link,
  cache: new InMemoryCache(),
});

export default client;
