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
                    Type = typeof(CircuitBreakerType),
                    Resolver = new FuncFieldResolver<CircuitBreakerEntity>(ResolveCircuitBreaker),
                    Subscriber = new EventStreamResolver<CircuitBreakerEntity>(Subscribe)
                });
        }

        private CircuitBreakerEntity ResolveCircuitBreaker(ResolveFieldContext context)
        {
            return context.Source as CircuitBreakerEntity;
        }

        private IObservable<CircuitBreakerEntity> Subscribe(ResolveEventStreamContext context)
        {
            return null;
        }
    }
}
