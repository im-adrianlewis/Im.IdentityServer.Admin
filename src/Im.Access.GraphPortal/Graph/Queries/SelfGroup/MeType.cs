using GraphQL;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.SelfGroup
{
    public class MeType : ObjectGraphType<UserEntity>
    {
        public MeType(IDependencyResolver dependencyResolver)
        {
            Name = "Me";
            Description = "Object for querying information about the current caller.";

            Field(u => u.Id).Description("User identifier");
            Field(u => u.TenantId).Description("Tenant associated with the user account");
            Field(u => u.FirstName, true).Description("User's first name");
            Field(u => u.LastName, true).Description("User's last name");
            Field(u => u.Email, true).Description("User's email address");
            Field(u => u.EmailConfirmed).Description("Flag indicating whether email is confirmed");
        }
    }
}
