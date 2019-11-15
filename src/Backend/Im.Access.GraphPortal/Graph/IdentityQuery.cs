using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.ClientGroup.Queries;
using Im.Access.GraphPortal.Graph.OperationalGroup.Queries;
using Im.Access.GraphPortal.Graph.UserGroup.Queries;

namespace Im.Access.GraphPortal.Graph
{
    public class IdentityQuery : ObjectGraphType
    {
        public IdentityQuery(IDependencyResolver dependencyResolver)
        {
            Name = "IdentityQuery";
            Description = "Root for all query access.";

            Field<UserQueryType>(
                "user",
                "Access information associated with users.",
                resolve: fieldContext => dependencyResolver.Resolve<UserQueryType>());

            Field<ClientQueryType>(
                "client",
                "Access information associated with clients.",
                resolve: fieldContext => dependencyResolver.Resolve<ClientQueryType>());

            Field<OperationalQueryType>(
                "operations",
                "Access information associated with service operations.",
                resolve: fieldContext => dependencyResolver.Resolve<OperationalQueryType>());
        }
    }
}
