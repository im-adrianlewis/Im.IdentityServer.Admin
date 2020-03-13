using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Data
{
    public interface IChaosPolicyStore
    {
        Task<IEnumerable<DbChaosPolicy>> GetChaosPoliciesAsync(CancellationToken cancellationToken);

        Task<DbChaosPolicy> GetChaosPolicyAsync(
            Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<DbChaosPolicy>> GetChaosPoliciesChangedSinceAsync(DateTimeOffset changesSince, CancellationToken cancellationToken);
    }
}