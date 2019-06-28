using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.TenantGroup
{
    public class UserClaimType : ObjectGraphType<UserClaimEntity>
    {
        public UserClaimType()
        {
            Name = "UserClaim";
            Description = "Represents a given user's claim.";

            Field(c => c.ClaimType).Description("Claim type");
            Field(c => c.ClaimValue).Description("Claim value");
            Field(c => c.ClaimUpdatedDate).Description("Date when claim value was last updated.");
        }
    }
}