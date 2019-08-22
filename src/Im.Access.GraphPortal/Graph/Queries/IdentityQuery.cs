using System.Security.Claims;
using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Queries.SelfGroup;
using Im.Access.GraphPortal.Graph.Queries.TenantGroup;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries
{
    public class IdentityQuery : ObjectGraphType
    {
        public IdentityQuery(IDependencyResolver dependencyResolver)
        {
            Name = "IdentityQuery";
            Description = "Root for all query access.";

            Field<TenantType>(
                "tenant",
                "Access information for a specific tenant.",
                new QueryArguments(
                    new QueryArgument(typeof(StringGraphType))
                    {
                        Name = "tenantId"
                    }),
                fieldContext =>
                {
                    var tenantType = dependencyResolver.Resolve<TenantType>();
                    tenantType.TenantId = fieldContext.GetArgument<string>("tenantId");
                    return tenantType;
                });

            FieldAsync<MeType>(
                "me",
                "Access to information for the current caller.",
                resolve: async (fieldContext) =>
                {
                    var userRepo = dependencyResolver.Resolve<IUserRepository>();
                    return await userRepo
                        .GetSelfAsync(
                            fieldContext.UserContext as ClaimsPrincipal,
                            fieldContext.CancellationToken)
                        .ConfigureAwait(false);
                });
        }
    }
}
