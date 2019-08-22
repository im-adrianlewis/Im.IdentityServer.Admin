using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Im.Access.GraphPortal.Data
{
    public class UserStore : IUserStore
    {
        private readonly IdentityDbContext _context;

        public UserStore(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationResult<DbUser>> GetUsersAsync(
            UserSearchCriteria criteria,
            CancellationToken cancellationToken)
        {
            // Build user query
            IQueryable<DbUser> query = _context.Users;
            if (!string.IsNullOrWhiteSpace(criteria.TenantId))
            {
                query = query.Where(e => e.TenantId == criteria.TenantId);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FirstName))
            {
                query = query.Where(e => e.FirstName.Contains(criteria.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(criteria.LastName))
            {
                query = query.Where(e => e.LastName.Contains(criteria.LastName));
            }

            if (!string.IsNullOrWhiteSpace(criteria.Email))
            {
                query = query.Where(e => e.Email.Contains(criteria.Email));
            }

            if (!string.IsNullOrWhiteSpace(criteria.ScreenName))
            {
                query = query.Where(e => e.ScreenName.Contains(criteria.ScreenName));
            }

            if (criteria.CreateDateFrom.HasValue)
            {
                query = query.Where(e => e.CreateDate >= criteria.CreateDateFrom);
            }

            if (criteria.CreateDateTo.HasValue)
            {
                query = query.Where(e => e.CreateDate <= criteria.CreateDateTo);
            }

            if (criteria.LastLoggedInDateFrom.HasValue)
            {
                query = query.Where(e => e.LastLoggedInDate >= criteria.LastLoggedInDateFrom);
            }

            if (criteria.LastLoggedInDateTo.HasValue)
            {
                query = query.Where(e => e.LastLoggedInDate <= criteria.LastLoggedInDateTo);
            }

            var count = await query
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            var items = await query
                .Include(e => e.Claims)
                .OrderBy(e => e.TenantId)
                .ThenBy(e => e.Email)
                .Skip(criteria.PageIndex * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return
                new PaginationResult<DbUser>
                {
                    TotalCount = count,
                    PageIndex = criteria.PageIndex,
                    PageSize = criteria.PageSize,
                    Items = items
                };
        }

        public Task<DbUser> GetUserAsync(
            string userId, CancellationToken cancellationToken)
        {
            return _context
                .Users
                .FindAsync(
                    new[]
                    {
                        userId
                    },
                    cancellationToken);
        }
    }
}
