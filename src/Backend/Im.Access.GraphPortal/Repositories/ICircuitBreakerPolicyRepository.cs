using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface ICircuitBreakerPolicyRepository
    {
        Task<IEnumerable<CircuitBreakerPolicyEntity>> GetCircuitBreakersAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);

        Task<CircuitBreakerPolicyEntity> UpdateCircuitBreaker(
            ClaimsPrincipal user,
            CircuitBreakerInput breaker,
            CancellationToken cancellationToken);
    }
}