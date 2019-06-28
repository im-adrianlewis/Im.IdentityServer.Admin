using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Graph.Queries.TenantGroup;

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
                fieldResolver =>
                {
                    var tenantType = dependencyResolver.Resolve<TenantType>();
                    tenantType.TenantId = fieldResolver.GetArgument<string>("tenantId");
                    return tenantType;
                });
        }
    }
}
