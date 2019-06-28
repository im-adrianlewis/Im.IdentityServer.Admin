using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Data
{
    public interface IUserStore
    {
        Task<PaginationResult<DbUser>> GetUsersAsync(
            UserSearchCriteria criteria,
            CancellationToken cancellationToken);
    }
}