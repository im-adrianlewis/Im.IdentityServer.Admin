using System;
using System.Collections.Generic;
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

            return null;
        }
    }
}
