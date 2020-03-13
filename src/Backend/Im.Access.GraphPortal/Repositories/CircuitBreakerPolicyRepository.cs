using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class CircuitBreakerPolicyRepository : ICircuitBreakerPolicyRepository
    {
        private readonly ICircuitBreakerPolicyStore _circuitBreakerPolicyStore;
        private CircuitBreakerPolicySubscriptionManager _subscriptionManager;

        public CircuitBreakerPolicyRepository(ICircuitBreakerPolicyStore circuitBreakerPolicyStore)
        {
            _circuitBreakerPolicyStore = circuitBreakerPolicyStore;
        }

        public async Task<IEnumerable<CircuitBreakerPolicyEntity>> GetAllAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken)
        {
            var rawPolicies = await _circuitBreakerPolicyStore
                .GetCircuitBreakerPoliciesAsync(cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            return rawPolicies
                .Select(p =>
                    new CircuitBreakerPolicyEntity
                    {
                        Id = p.Id,
                        PolicyKey = p.PolicyKey,
                        Service = p.ServiceName,
                        IsIsolated = p.IsIsolated,
                        LastUpdated = p.LastUpdated
                    });
        }

        public Task<CircuitBreakerPolicyEntity> UpdateAsync(
            ClaimsPrincipal user,
            CircuitBreakerPolicyInput circuitBreakerPolicy,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IObservable<CircuitBreakerPolicyEntity> Subscribe(
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            if (_subscriptionManager == null)
            {
                _subscriptionManager = new CircuitBreakerPolicySubscriptionManager(
                    _circuitBreakerPolicyStore, CancellationToken.None);
            }

            return _subscriptionManager;
        }
    }
}
