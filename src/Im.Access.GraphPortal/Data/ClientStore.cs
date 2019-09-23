using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Data
{
    public class ClientStore : IClientStore
    {
        private readonly ConfigurationDbContext _context;

        public ClientStore(ConfigurationDbContext context)
        {
            _context = context;
        }

        public Task<PaginationResult<DbClient>> GetClientsAsync(
            ClientSearchCriteria criteria, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}