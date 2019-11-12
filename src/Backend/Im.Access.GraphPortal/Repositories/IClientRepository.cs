using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IClientRepository
    {
        Task<PaginationResult<ClientEntity>> GetClientsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            ClientSearchCriteria clientSearchCriteria);
    }
}