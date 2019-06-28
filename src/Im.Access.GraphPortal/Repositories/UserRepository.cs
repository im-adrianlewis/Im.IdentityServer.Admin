using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IUserRepository
    {
        Task<PaginationResult<UserEntity>> GetUsersAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken,
            UserSearchCriteria searchCriteria);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IUserStore _userStore;

        public UserRepository(IUserStore userStore)
        {
            _userStore = userStore;
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

            if (!user.IsInRole("superadmin") &&
                user.IsInRole("admin") && (string.IsNullOrWhiteSpace(searchCriteria.TenantId) ||
                                           !user.HasClaim("Tenant", searchCriteria.TenantId)))
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
    }

    public class PaginationResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }

    public class UserSearchCriteria
    {
        public string TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ScreenName { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo { get; set; }

        public DateTime? LastLoggedInDateFrom { get; set; }

        public DateTime? LastLoggedInDateTo { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}