using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Im.Access.GraphPortal.Data
{
    public class ClientStore : IClientStore
    {
        private readonly ConfigurationDbContext _context;

        public ClientStore(ConfigurationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationResult<DbClient>> GetClientsAsync(
            ClientSearchCriteria criteria, CancellationToken cancellationToken)
        {
            IQueryable<DbClient> query = _context
                .Clients
                .Include(c => c.Claims);

            if (!string.IsNullOrEmpty(criteria.ClientId))
            {
                query = query.Where(c => c.ClientId == criteria.ClientId);
            }

            if (!string.IsNullOrEmpty(criteria.TenantId))
            {
                query = query.Where(c => c.Claims.Any(
                    cl => cl.Type == "Tenant" && cl.Value == criteria.TenantId));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var subItems = await query
                .Skip(criteria.PageIndex * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToArrayAsync(cancellationToken);
            return new PaginationResult<DbClient>
            {
                PageIndex = criteria.PageIndex,
                PageSize = criteria.PageSize,
                TotalCount = totalCount,
                Items = subItems
            };
        }
    }
}