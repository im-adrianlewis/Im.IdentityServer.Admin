import { HttpLink, ApolloClient, InMemoryCache } from 'apollo-boost';
import fetch from 'isomorphic-fetch';
import { GRAPHQL_ENDPOINT } from '..//constants/env';

const link = new HttpLink({
  uri: GRAPHQL_ENDPOINT,
  fetch
});

const client = new ApolloClient({
  link: link,
  cache: new InMemoryCache(),
});

export default client;
