using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.TenantGroup
{
    public class UserType : ObjectGraphType<UserEntity>
    {
        public UserType()
        {
            Name = "User";
            Description = "Represents a single user.";

            Field(u => u.Id).Description("User identifier");
            Field(u => u.TenantId).Description("Tenant associated with the user account");
            Field(u => u.FirstName, true).Description("User's first name");
            Field(u => u.LastName, true).Description("User's last name");
            Field(u => u.Email, true).Description("User's email address");
            Field(u => u.EmailConfirmed).Description("Flag indicating whether email is confirmed");
            Field<ListGraphType<UserClaimType>>("Claims", "User's claims");
        }
    }
}