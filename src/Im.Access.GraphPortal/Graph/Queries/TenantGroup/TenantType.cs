using GraphQL;
using GraphQL.Types;

namespace Im.Access.GraphPortal.Graph.Queries.TenantGroup
{
    public class TenantType : ObjectGraphType
    {
        public TenantType(IDependencyResolver dependencyResolver)
        {
            Name = "Tenant";
            Description = "Encapsulates query operations associated with a specific tenant.";

            Field(
                typeof(StringGraphType),
                "TenantId",
                "Gets the tenant identifier.",
                resolve: _ => TenantId);
            Field<UserTenantType>(
                "Users",
                "Gains access to user account settings.",
                resolve: _ =>
                {
                    var userTenantType = dependencyResolver.Resolve<UserTenantType>();
                    userTenantType.TenantId = TenantId;
                    return userTenantType;
                });
        }

        internal string TenantId { get; set; }
    }
}