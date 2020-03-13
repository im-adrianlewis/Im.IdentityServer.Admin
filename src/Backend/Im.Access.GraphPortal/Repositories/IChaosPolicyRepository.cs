using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IChaosPolicyRepository
    {
        Task<IEnumerable<ChaosPolicyEntity>> GetAllAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);

        Task<ChaosPolicyEntity> UpdateAsync(
            ClaimsPrincipal contextUserContext,
            ChaosPolicyInput chaosPolicy,
            CancellationToken contextCancellationToken);

        IObservable<ChaosPolicyEntity> Subscribe(
            ClaimsPrincipal user,
            CancellationToken cancellationToken);
    }
}