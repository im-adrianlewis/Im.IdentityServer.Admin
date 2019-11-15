using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public Task<CircuitBreakerEntity> UpdateCircuitBreaker(ClaimsPrincipal user, CircuitBreakerInput breaker, CancellationToken cancellationToken)
        {
            ICircuitBreakerPolicy policy;
            if (!_policyRegistry.TryGet(breaker.Name, out policy))
            {
                throw new Exception("Circuit breaker policy not found");
            }

            if (breaker.State == CircuitBreakerInputState.Isolate)
            {
                policy.Isolate();
            }
            else if (breaker.State == CircuitBreakerInputState.Reset)
            {
                policy.Reset();
            }

            return Task.FromResult(
                new CircuitBreakerEntity
                {
                    Name = breaker.Name,
                    State = policy.CircuitState.ToString()
                });
        }
    }
}
