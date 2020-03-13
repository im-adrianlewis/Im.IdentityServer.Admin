using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface ICircuitBreakerPolicyRepository
    {
        Task<IEnumerable<CircuitBreakerPolicyEntity>> GetAllAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);

        Task<CircuitBreakerPolicyEntity> UpdateAsync(
            ClaimsPrincipal user,
            CircuitBreakerPolicyInput circuitBreakerPolicy,
            CancellationToken cancellationToken);

        IObservable<CircuitBreakerPolicyEntity> Subscribe(
            ClaimsPrincipal user,
            CancellationToken cancellationToken);
    }
}