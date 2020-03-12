using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class OperationalMutationType : ObjectGraphType
    {
        public OperationalMutationType(ICircuitBreakerPolicyRepository circuitBreakerPolicyRepository)
        {
            FieldAsync<CircuitBreakerPolicyType>(
                "updateCircuitBreaker",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CircuitBreakerPolicyInputType>>
                    {
                        Name = "circuitBreaker"
                    }),
                resolve: async (context) =>
                {
                    var breaker = context.GetArgument<CircuitBreakerInput>("circuitBreaker");
                    return await circuitBreakerPolicyRepository.UpdateCircuitBreaker(
                        context.UserContext as ClaimsPrincipal,
                        breaker,
                        context.CancellationToken);
                });
        }
    }
}
