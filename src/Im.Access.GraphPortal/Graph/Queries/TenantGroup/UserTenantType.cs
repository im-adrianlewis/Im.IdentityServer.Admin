using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.TenantGroup
{
    public class UserTenantType : ObjectGraphType
    {
        public UserTenantType(IUserRepository userRepository)
        {
            Name = "UserTenant";
            Description = "Query operations scoped to a single tenant.";

            FieldAsync<PaginationType<UserType, UserEntity>>(
                "find",
                "Searches for users that match specified criteria.",
                new QueryArguments
                {
                    new QueryArgument(typeof(UserSearchCriteriaType))
                    {
                        Name = "criteria",
                        Description = "Defines the user search criteria."
                    }
                },
                resolve: async fieldResolver =>
                {
                    var searchCriteria = fieldResolver.GetArgument<UserSearchCriteria>("criteria");
                    searchCriteria.TenantId = TenantId;

                    // ReSharper disable once ConvertToLambdaExpression
                    return await userRepository
                        .GetUsersAsync(
                            (ClaimsPrincipal)fieldResolver.UserContext,
                            fieldResolver.CancellationToken,
                            searchCriteria)
                        .ConfigureAwait(false);
                });
        }

        internal string TenantId { get; set; }
    }
}