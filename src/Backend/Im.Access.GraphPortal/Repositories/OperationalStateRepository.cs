using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using Polly.CircuitBreaker;
using Polly.Registry;

namespace Im.Access.GraphPortal.Repositories
{
    public class OperationalStateRepository : IOperationalStateRepository
    {
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public OperationalStateRepository(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        public Task<IEnumerable<CircuitBreakerEntity>> GetCircuitBreakersAsync(ClaimsPrincipal user, string filter, CancellationToken cancellationToken)
        {
            var entities = _policyRegistry
                .Select(pair =>
                    new
                    {
                        Name = pair.Key,
                        Policy = pair.Value.As<ICircuitBreakerPolicy>()
                    })
                .Where(pair => pair.Policy != null)
                .Select(pair =>
                    new CircuitBreakerEntity
                    {
                        Name = pair.Name,
                        State = pair.Policy.CircuitState.ToString()
                    });
            return Task.FromResult(entities);
        }
    }
}
