using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.ClientGroup
{
    public class ClientQueryType : ObjectGraphType
    {
        public ClientQueryType(IClientRepository clientRepository)
        {
            Name = "Clients";
            Description = "Query operations scoped to client operations.";

        }
    }

    public class ClientType : ObjectGraphType<ClientEntity>
    {

    }
}
