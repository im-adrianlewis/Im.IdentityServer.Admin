using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Common.Queries;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.ClientGroup.Queries
{
    public class ClientQueryType : ObjectGraphType
    {
        public ClientQueryType(IClientRepository clientRepository)
        {
            Name = "Clients";
            Description = "Query operations scoped to client operations.";

            FieldAsync<PaginationType<ClientType, ClientEntity>>(
                "find",
                "Gets a list of clients",
                resolve: async (fieldContext) =>
                {
                    return await clientRepository.GetClientsAsync(
                        fieldContext.UserContext as ClaimsPrincipal,
                        fieldContext.CancellationToken,
                        null);
                });
        }
    }
}
