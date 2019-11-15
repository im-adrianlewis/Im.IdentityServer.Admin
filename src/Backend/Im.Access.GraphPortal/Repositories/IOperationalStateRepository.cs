using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IOperationalStateRepository
    {
        Task<IEnumerable<CircuitBreakerEntity>> GetCircuitBreakersAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);

        Task<CircuitBreakerEntity> UpdateCircuitBreaker(
            ClaimsPrincipal user,
            CircuitBreakerInput breaker,
            CancellationToken cancellationToken);
    }
}