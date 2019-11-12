using System.Security.Claims;

namespace Im.Access.GraphPortal.Repositories
{
    public static class PermissionCheck
    {
        public static bool HasAdminPermission(ClaimsPrincipal user, string tenantId)
        {
            if (user.IsInRole(IdentityConstants.Role.SuperAdministrator))
            {
                return true;
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                return false;
            }

            if (user.HasClaim(IdentityConstants.Claim.Administrator, tenantId))
            {
                return true;
            }

            return false;
        }
    }
}