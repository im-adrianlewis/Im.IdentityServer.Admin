import fetch from 'isomorphic-fetch';
import { GRAPHQL_REGULAR_ENDPOINT, GRAPHQL_SUBSCRIPTION_ENDPOINT } from '..//constants/env';
import { split } from 'apollo-link';
import { HttpLink } from 'apollo-link-http';
import { WebSocketLink } from 'apollo-link-ws';
import { getMainDefinition } from 'apollo-utilities';
import { ApolloClient, InMemoryCache } from 'apollo-boost';
import { SubscriptionClient } from 'subscriptions-transport-ws';
import ws from 'ws';

var isServer = (typeof window === 'undefined');
console.log(`Server: ${isServer}, Graph: ${GRAPHQL_REGULAR_ENDPOINT}, Subs: ${GRAPHQL_SUBSCRIPTION_ENDPOINT}`);

const httpLink = new HttpLink({
  uri: GRAPHQL_REGULAR_ENDPOINT,
  fetch
});

const subscriptionClient = new SubscriptionClient(
  GRAPHQL_SUBSCRIPTION_ENDPOINT ?? 'ws://localhost:44345', {
    reconnect: true
  },
  isServer ? ws : undefined);

const wsLink = new WebSocketLink(subscriptionClient);

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
