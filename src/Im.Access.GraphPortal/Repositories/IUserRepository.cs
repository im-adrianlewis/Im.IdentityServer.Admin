using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> GetSelfAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken);

        Task<PaginationResult<UserEntity>> GetUsersAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            UserSearchCriteria searchCriteria);
    }
}