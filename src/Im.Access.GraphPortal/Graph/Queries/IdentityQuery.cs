using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Queries.UserGroup;

namespace Im.Access.GraphPortal.Graph.Queries
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
                resolve: fieldContext =>
                {
                    return dependencyResolver.Resolve<UserQueryType>();
                });

        }
    }
}
