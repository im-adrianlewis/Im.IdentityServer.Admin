using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Queries
{
    public class OperationalQueryType : ObjectGraphType
    {
        public OperationalQueryType(IOperationalStateRepository operationalStateRepository)
        {
            Name = "Operations";
            Description = "Query operations scoped to service operations";

            FieldAsync<ListGraphType<CircuitBreakerType>>(
                "CircuitBreakers",
                "Gets the circuit breaker states for the service",
                resolve: async (fieldContext) =>
                {
                    return await operationalStateRepository
                        .GetCircuitBreakersAsync(
                            fieldContext.UserContext as ClaimsPrincipal,
                            "",
                            fieldContext.CancellationToken);
                });
        }
    }
}
