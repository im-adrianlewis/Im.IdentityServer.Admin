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
            Field<CircuitBreakerPolicyType>(
                "circuitBreakerPolicyEvents",
                resolve: (context) =>
                {
                    return new CircuitBreakerPolicySubscriptionType(circuitBreakerPolicyRepository);
                });

            Field<ChaosPolicyType>(
                "chaosPolicyEvents",
                resolve: (context) =>
                {
                    return new ChaosPolicySubscriptionType(chaosPolicyRepository);
                });
        }
    }
}
