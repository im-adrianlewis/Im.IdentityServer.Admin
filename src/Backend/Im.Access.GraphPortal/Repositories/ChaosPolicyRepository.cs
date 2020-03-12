﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class ChaosPolicyRepository : IChaosPolicyRepository
    {
        private readonly IChaosPolicyStore _chaosPolicyStore;

        public ChaosPolicyRepository(IChaosPolicyStore chaosPolicyStore)
        {
            _chaosPolicyStore = chaosPolicyStore;
        }

        public async Task<IEnumerable<ChaosPolicyEntity>> GetChaosPoliciesAsync(ClaimsPrincipal user, string filter, CancellationToken cancellationToken)
        {
            var rawPolicies = await _chaosPolicyStore
                .GetChaosPoliciesAsync(cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            return rawPolicies
                .Select(p =>
                    new ChaosPolicyEntity
                    {
                        Id = p.Id,
                        PolicyKey = p.PolicyKey,
                        Service = p.ServiceName,
                        Enabled = p.Enabled,
                        FaultEnabled = p.FaultEnabled,
                        FaultInjectionRate = p.FaultInjectionRate,
                        LatencyEnabled = p.LatencyEnabled,
                        LatencyInjectionRate = p.LatencyInjectionRate,
                        LastUpdated = p.LastUpdated
                    });
        }

        //public Task<CircuitBreakerEntity> UpdateCircuitBreaker(ClaimsPrincipal user, CircuitBreakerInput breaker, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}