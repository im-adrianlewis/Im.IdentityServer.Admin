using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserStore _userStore;

        public UserRepository(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task<UserEntity> GetSelfAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            if (!user.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("Missing user context");
            }

            var sub = user.FindFirst(ClaimTypes.NameIdentifier);
            var result = await _userStore
                .GetUserAsync(sub.Value, cancellationToken)
                .ConfigureAwait(false);

            return new UserEntity(result);
        }

        public async Task<PaginationResult<UserEntity>> GetUsersAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            UserSearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            if (!CanAccessTenant(user, searchCriteria.TenantId))
            {
                // TODO: Strong-type for authorization exception
                throw new Exception("Access denied");
            }

            var result = await _userStore
                .GetUsersAsync(searchCriteria, cancellationToken)
                .ConfigureAwait(false);
            return new PaginationResult<UserEntity>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Items = result.Items.Select(e => new UserEntity(e))
            };
        }

        private bool CanAccessTenant(ClaimsPrincipal user, string tenantId)
        {
            if (user.IsInRole("superadmin"))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return false;
            }

            if (user.IsInRole("admin") && user.HasClaim("Tenant", tenantId))
            {
                return true;
            }

            return false;
        }
    }
}