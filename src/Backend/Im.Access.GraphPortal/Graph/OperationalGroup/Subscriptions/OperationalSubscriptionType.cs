using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions
{
    public class OperationalSubscriptionType : ObjectGraphType
    {
        public OperationalSubscriptionType(IOperationalStateRepository operationalStateRepository)
        {
            Field<CircuitBreakerType>(
                "eventsCircuitBreaker",
                resolve: (context) =>
                {
                    return new CircuitBreakerSubscriptions();
                });
        }
    }
}
