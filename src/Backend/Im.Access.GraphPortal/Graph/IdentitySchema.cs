using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Mutations;
using Im.Access.GraphPortal.Graph.Queries;
using Im.Access.GraphPortal.Graph.Subscriptions;

namespace Im.Access.GraphPortal.Graph
{
    public class IdentitySchema : Schema
    {
        public IdentitySchema(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<IdentityQuery>();
            //Mutation = dependencyResolver.Resolve<IdentityMutation>();
            //Subscription = dependencyResolver.Resolve<IdentitySubscription>();
        }
    }
}
