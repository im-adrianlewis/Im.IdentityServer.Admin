# Im.Access.Graph
GraphQL endpoint for accessing identity data store

## Intro
This project is comprised of two parts; frontend and backend.

### Backend
This is a .NET Core 3.1 project that exposes a GraphQL endpoint with the aim being 100% data coverage of all tables used in the identity database schema.

Access to data will be controlled by the rights associated with access token supplied with each call. Additionally all calls to GraphQL will be audited.

Currently; query, mutations and subscriptions are implemented within the code-base with the code organised into what I hope is something vaguely sensible!

### Frontend
This is a NodeJS/Express/NextJS application built using React, Redux Saga, Passport. I have successfully integrated the logic needed to allow GraphQL subscriptions over web-sockets (however this hasn't been validated)

### Authentication
Although not part of the solution itself, this project relies upon Im.Access to handle authentication/authorization and the mechanism for how to achieve this from a NodeJS point of view is in place.