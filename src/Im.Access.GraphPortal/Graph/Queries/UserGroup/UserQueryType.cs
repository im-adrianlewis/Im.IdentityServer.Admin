using System.Security.Claims;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.UserGroup
{
    public class UserQueryType : ObjectGraphType
    {
        public UserQueryType(IUserRepository userRepository)
        {
            Name = "Users";
            Description = "Query operations scoped to user operations.";

            FieldAsync<UserType>(
                "me",
                "Access to information for the current caller.",
                resolve: async (fieldContext) =>
                {
                    return await userRepository
                        .GetSelfAsync(
                            fieldContext.UserContext as ClaimsPrincipal,
                            fieldContext.CancellationToken)
                        .ConfigureAwait(false);
                });

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

                    // ReSharper disable once ConvertToLambdaExpression
                    return await userRepository
                        .GetUsersAsync(
                            (ClaimsPrincipal)fieldResolver.UserContext,
                            fieldResolver.CancellationToken,
                            searchCriteria)
                        .ConfigureAwait(false);
                });
        }
    }
}