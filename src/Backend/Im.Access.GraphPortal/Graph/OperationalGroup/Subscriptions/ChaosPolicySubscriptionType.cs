using System;
using System.Security.Claims;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions
{
    public class ChaosPolicySubscriptionType : ObjectGraphType
    {
        private readonly IChaosPolicyRepository _chaosPolicyRepository;

        public ChaosPolicySubscriptionType(
            IChaosPolicyRepository chaosPolicyRepository)
        {
            _chaosPolicyRepository = chaosPolicyRepository;

            AddField(
                new EventStreamFieldType
                {
                    Name = "chaosPolicyChanged",
                    Type = typeof(ChaosPolicyType),
                    Resolver = new FuncFieldResolver<ChaosPolicyEntity>(ResolveChaosPolicy),
                    Subscriber = new EventStreamResolver<ChaosPolicyEntity>(Subscribe)
                });
        }

        private ChaosPolicyEntity ResolveChaosPolicy(ResolveFieldContext context)
        {
            return context.Source as ChaosPolicyEntity;
        }

        private IObservable<ChaosPolicyEntity> Subscribe(ResolveEventStreamContext context)
        {
            return _chaosPolicyRepository.Subscribe(
                (ClaimsPrincipal)context.UserContext,
                context.CancellationToken);
        }
    }
}