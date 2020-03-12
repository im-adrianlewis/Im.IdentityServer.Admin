using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions
{
    public class CircuitBreakerSubscriptions : ObjectGraphType
    {
        public CircuitBreakerSubscriptions()
        {
            // TODO: Store reference to circuit-breaker store

            AddField(
                new EventStreamFieldType
                {
                    Name = "circuitBreakerChanged",
                    Type = typeof(CircuitBreakerPolicyType),
                    Resolver = new FuncFieldResolver<CircuitBreakerPolicyEntity>(ResolveCircuitBreaker),
                    Subscriber = new EventStreamResolver<CircuitBreakerPolicyEntity>(Subscribe)
                });
        }

        private CircuitBreakerPolicyEntity ResolveCircuitBreaker(ResolveFieldContext context)
        {
            return context.Source as CircuitBreakerPolicyEntity;
        }

        private IObservable<CircuitBreakerPolicyEntity> Subscribe(ResolveEventStreamContext context)
        {
            return null;
        }
    }
}
