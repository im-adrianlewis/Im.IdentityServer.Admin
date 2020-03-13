using System;
using System.Security.Claims;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions
{
    public class CircuitBreakerPolicySubscriptionType : ObjectGraphType
    {
        private readonly ICircuitBreakerPolicyRepository _circuitBreakerPolicyRepository;

        public CircuitBreakerPolicySubscriptionType(
            ICircuitBreakerPolicyRepository circuitBreakerPolicyRepository)
        {
            _circuitBreakerPolicyRepository = circuitBreakerPolicyRepository;

            AddField(
                new EventStreamFieldType
                {
                    Name = "circuitBreakerPolicyChanged",
                    Type = typeof(CircuitBreakerPolicyType),
                    Resolver = new FuncFieldResolver<CircuitBreakerPolicyEntity>(ResolveCircuitBreakerPolicy),
                    Subscriber = new EventStreamResolver<CircuitBreakerPolicyEntity>(Subscribe)
                });
        }

        private CircuitBreakerPolicyEntity ResolveCircuitBreakerPolicy(ResolveFieldContext context)
        {
            return context.Source as CircuitBreakerPolicyEntity;
        }

        private IObservable<CircuitBreakerPolicyEntity> Subscribe(ResolveEventStreamContext context)
        {
            return _circuitBreakerPolicyRepository.Subscribe(
                (ClaimsPrincipal)context.UserContext,
                context.CancellationToken);
        }
    }
}
