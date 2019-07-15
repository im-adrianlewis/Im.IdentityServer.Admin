using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IUserRepository
    {
        Task<PaginationResult<UserEntity>> GetUsersAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            UserSearchCriteria searchCriteria);
    }
}