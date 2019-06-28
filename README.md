# Im.Access.Graph
GraphQL endpoint for accessing identity data store

## Intro
This is a .NET Core 2.2 project that exposes a GraphQL endpoint with the aim being 100% data coverage of all tables used in the identity database schema.

Access to data will be controlled by the rights associated with access token supplied with each call. Additionally all calls to GraphQL will be audited.

In time support will be added for GraphQL mutations (first with the /me endpoint for individual user settings and extending into user and claims areas for admin support)

Finally subscription support will be added so we can have real-time updates pushed to subscribers.
