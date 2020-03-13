using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Queries
{
    public class OperationalQueryType : ObjectGraphType
    {
        public OperationalQueryType(
            ICircuitBreakerPolicyRepository circuitBreakerPolicyRepository,
            IChaosPolicyRepository chaosPolicyRepository)
        {
            Name = "Operations";
            Description = "Query operations scoped to service operations";

            FieldAsync<ListGraphType<CircuitBreakerPolicyType>>(
                "CircuitBreakerPolicies",
                "Gets the circuit breaker policies.",
                resolve: async (fieldContext) =>
                {
                    return await circuitBreakerPolicyRepository
                        .GetAllAsync(
                            fieldContext.UserContext as ClaimsPrincipal,
                            "",
                            fieldContext.CancellationToken);
                });

            FieldAsync<ListGraphType<ChaosPolicyType>>(
                "ChaosPolicies",
                "Gets the chaos policies.",
                resolve: async (fieldContext) =>
                {
                    return await chaosPolicyRepository
                        .GetAllAsync(
                            fieldContext.UserContext as ClaimsPrincipal,
                            "",
                            fieldContext.CancellationToken);
                });
        }
    }
}
