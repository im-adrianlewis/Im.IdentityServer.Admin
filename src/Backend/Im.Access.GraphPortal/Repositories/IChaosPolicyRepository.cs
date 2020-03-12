using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IChaosPolicyRepository
    {
        Task<IEnumerable<ChaosPolicyEntity>> GetChaosPoliciesAsync(
            ClaimsPrincipal user,
            string filter,
            CancellationToken cancellationToken);

        //Task<ChaosPolicyEntity> UpdateChaosPolicy(
        //    ClaimsPrincipal user,
        //    ChaosPolicyInput chaos,
        //    CancellationToken cancellationToken);
    }
}