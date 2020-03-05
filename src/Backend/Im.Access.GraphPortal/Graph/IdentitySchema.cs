using GraphQL;
using GraphQL.Types;

namespace Im.Access.GraphPortal.Graph
{
    public class IdentitySchema : Schema
    {
        public IdentitySchema(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<IdentityQuery>();
            Mutation = dependencyResolver.Resolve<IdentityMutation>();
            Subscription = dependencyResolver.Resolve<IdentitySubscription>();
        }
    }
}
