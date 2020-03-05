using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions;

namespace Im.Access.GraphPortal.Graph
{
    public class IdentitySubscription : ObjectGraphType
    {
        public IdentitySubscription(IDependencyResolver dependencyResolver)
        {
            Name = "IdentitySubscription";
            Description = "Root for all subscription access";

            Field<OperationalSubscriptionType>(
                "operations",
                "Access service operational controls.",
                resolve: fieldContext => dependencyResolver.Resolve<OperationalSubscriptionType>());
        }
    }
}
