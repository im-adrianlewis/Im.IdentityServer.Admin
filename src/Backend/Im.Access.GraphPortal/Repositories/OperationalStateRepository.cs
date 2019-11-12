using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IOperationalStateRepository
    {
        Task<IEnumerable<CircuitBreakerEntity>> GetCircuitBreakersAsync(ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);
    }

    public class OperationalStateRepository : IOperationalStateRepository
    {
        public async Task<IEnumerable<CircuitBreakerEntity>> GetCircuitBreakersAsync(ClaimsPrincipal user, string filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CircuitBreakerEntity
    {
        public string Name { get; set; }

        public string State { get; set; }
    }
}
