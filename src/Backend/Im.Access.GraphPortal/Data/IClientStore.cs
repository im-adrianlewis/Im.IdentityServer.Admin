using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Data
{
    public interface IClientStore
    {
        Task<PaginationResult<DbClient>> GetClientsAsync(
            ClientSearchCriteria criteria,
            CancellationToken cancellationToken);
    }
}