using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Queries.ClientGroup;
using Im.Access.GraphPortal.Graph.Queries.UserGroup;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                resolve: fieldContext => dependencyResolver.Resolve<UserQueryType>());

            Field<ClientQueryType>(
                "client",
                "Access information associated with clients.",
                resolve: fieldContext => dependencyResolver.Resolve<ClientQueryType>());
        }
    }
}
