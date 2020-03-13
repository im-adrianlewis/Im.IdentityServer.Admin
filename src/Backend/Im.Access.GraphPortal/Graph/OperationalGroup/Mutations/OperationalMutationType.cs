using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Mutations
{
    public class OperationalMutationType : ObjectGraphType
    {
        public OperationalMutationType(
            ICircuitBreakerPolicyRepository circuitBreakerPolicyRepository,
            IChaosPolicyRepository chaosPolicyRepository)
        {
            FieldAsync<CircuitBreakerPolicyType>(
                "updateCircuitBreakerPolicy",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CircuitBreakerPolicyInputType>>
                    {
                        Name = "circuitBreakerPolicy"
                    }),
                resolve: async (context) =>
                {
                    var circuitBreakerPolicy = context.GetArgument<CircuitBreakerPolicyInput>("circuitBreakerPolicy");
                    return await circuitBreakerPolicyRepository.UpdateAsync(
                        context.UserContext as ClaimsPrincipal,
                        circuitBreakerPolicy,
                        context.CancellationToken);
                });

            FieldAsync<ChaosPolicyType>(
                "updateChaosPolicy",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ChaosPolicyInputType>>
                    {
                        Name = "chaosPolicy"
                    }),
                resolve: async (context) =>
                {
                    var chaosPolicy = context.GetArgument<ChaosPolicyInput>("chaosPolicy");
                    return await chaosPolicyRepository.UpdateAsync(
                        context.UserContext as ClaimsPrincipal,
                        chaosPolicy,
                        context.CancellationToken);
                });
        }
    }
}
