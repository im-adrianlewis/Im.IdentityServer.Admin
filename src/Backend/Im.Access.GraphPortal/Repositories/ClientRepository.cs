using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IClientStore _clientStore;

        public ClientRepository(IClientStore clientStore)
        {
            _clientStore = clientStore;
        }

        public async Task<PaginationResult<ClientEntity>> GetClientsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            ClientSearchCriteria clientSearchCriteria)
        {
            if (!user.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Missing user context");
            }

            if (!PermissionCheck.HasAdminPermission(user, clientSearchCriteria.TenantId))
            {
                throw new InvalidOperationException("Access denied");
            }

            var results = await _clientStore
                .GetClientsAsync(clientSearchCriteria, cancellationToken);
            return new PaginationResult<ClientEntity>
            {
                PageIndex = results.PageIndex,
                PageSize = results.PageSize,
                TotalCount = results.TotalCount,
                Items = results.Items.Select(i => new ClientEntity(i))
            };
        }
    }
}
