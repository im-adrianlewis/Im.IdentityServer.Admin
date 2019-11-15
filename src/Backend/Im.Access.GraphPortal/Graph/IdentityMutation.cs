using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.OperationalGroup.Mutations;

namespace Im.Access.GraphPortal.Graph
{
    public class IdentityMutation : ObjectGraphType
    {
        public IdentityMutation(IDependencyResolver dependencyResolver)
        {
            Name = "IdentityMutation";
            Description = "Root for all mutation access";

            Field<OperationalMutationType>(
                "operations",
                "Access service operational controls.",
                resolve: fieldContext => dependencyResolver.Resolve<OperationalMutationType>());
        }
    }
}
