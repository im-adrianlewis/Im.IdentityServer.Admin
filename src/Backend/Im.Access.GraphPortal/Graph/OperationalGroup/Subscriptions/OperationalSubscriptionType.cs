using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions
{
    public class OperationalSubscriptionType : ObjectGraphType
    {
        public OperationalSubscriptionType(
            ICircuitBreakerPolicyRepository circuitBreakerPolicyRepository,
            IChaosPolicyRepository chaosPolicyRepository)
        {
            Field<CircuitBreakerPolicySubscriptionType>(
                "circuitBreakerPolicyEvents",
                resolve: (context) =>
                {
                    return new CircuitBreakerPolicySubscriptionType(circuitBreakerPolicyRepository);
                });

            Field<ChaosPolicySubscriptionType>(
                "chaosPolicyEvents",
                resolve: (context) =>
                {
                    return new ChaosPolicySubscriptionType(chaosPolicyRepository);
                });
        }
    }
}
