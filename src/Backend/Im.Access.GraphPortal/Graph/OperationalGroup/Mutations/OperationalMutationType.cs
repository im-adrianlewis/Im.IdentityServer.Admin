using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class OperationalMutationType : ObjectGraphType
    {
        public OperationalMutationType(IOperationalStateRepository operationalStateRepository)
        {
            FieldAsync<CircuitBreakerType>(
                "updateCircuitBreaker",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CircuitBreakerInputType>>
                    {
                        Name = "circuitBreaker"
                    }),
                resolve: async (context) =>
                {
                    var breaker = context.GetArgument<CircuitBreakerInput>("circuitBreaker");
                    return await operationalStateRepository.UpdateCircuitBreaker(
                        context.UserContext as ClaimsPrincipal,
                        breaker,
                        context.CancellationToken);
                });
        }
    }
}
