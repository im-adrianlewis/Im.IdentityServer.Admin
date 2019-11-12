using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Registry;

namespace Im.Access.GraphPortal.Data
{
    public class ClientStore : IClientStore
    {
        private readonly ConfigurationDbContext _context;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public ClientStore(
            ConfigurationDbContext context,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _context = context;
            _policyRegistry = policyRegistry;
        }

        public Task<PaginationResult<DbClient>> GetClientsAsync(
            ClientSearchCriteria criteria, CancellationToken cancellationToken)
        {
            var policy = _policyRegistry["SqlConnection"] as AsyncPolicy;
            // ReSharper disable once PossibleNullReferenceException
            return policy.ExecuteAsync(
                async () =>
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
                });
        }
    }
}