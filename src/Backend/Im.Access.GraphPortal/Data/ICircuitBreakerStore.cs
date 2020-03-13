using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Data
{
    public interface ICircuitBreakerPolicyStore
    {
        Task<IEnumerable<DbCircuitBreakerPolicy>> GetCircuitBreakerPoliciesAsync(
            CancellationToken cancellationToken);

        Task<DbCircuitBreakerPolicy> GetCircuitBreakerPolicyAsync(
            Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<DbCircuitBreakerPolicy>> GetCircuitBreakerPoliciesChangedSinceAsync(
            DateTimeOffset changesSince, in CancellationToken cancellationToken);
    }
}