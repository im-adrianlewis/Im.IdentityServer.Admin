using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Registry;

namespace Im.Access.GraphPortal.Data
{
    public class ChaosPolicyStore : IChaosPolicyStore
    {
        private readonly OperationDbContext _context;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public ChaosPolicyStore(
            OperationDbContext context,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _context = context;
            _policyRegistry = policyRegistry;
        }

        public async Task<IEnumerable<DbChaosPolicy>> GetChaosPoliciesAsync(
            CancellationToken cancellationToken)
        {
            var policy = _policyRegistry["SqlConnection"] as AsyncPolicy;
            // ReSharper disable once PossibleNullReferenceException
            return await policy.ExecuteAsync(
                async () =>
                {
                    // Build user query
                    return await _context
                        .ChaosPolicies
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false);
                });
        }

        public Task<DbChaosPolicy> GetChaosPolicyAsync(
            Guid id, CancellationToken cancellationToken)
        {
            var policy = _policyRegistry["SqlConnection"] as AsyncPolicy;
            // ReSharper disable once PossibleNullReferenceException
            return policy.ExecuteAsync(
                () =>
                {
                    return _context
                        .ChaosPolicies
                        .Where(u => u.Id == id)
                        .FirstOrDefaultAsync(cancellationToken);
                });
        }

        public async Task<IEnumerable<DbChaosPolicy>> GetChaosPoliciesChangedSinceAsync(DateTimeOffset changesSince, CancellationToken cancellationToken)
        {
            var policy = _policyRegistry["SqlConnection"] as AsyncPolicy;
            // ReSharper disable once PossibleNullReferenceException
            return await policy.ExecuteAsync(
                async () =>
                {
                    // Build user query
                    return await _context
                        .ChaosPolicies
                        .Where(p => p.LastUpdated >= changesSince)
                        .ToListAsync(cancellationToken)
                        .ConfigureAwait(false);
                });
        }
    }
}
