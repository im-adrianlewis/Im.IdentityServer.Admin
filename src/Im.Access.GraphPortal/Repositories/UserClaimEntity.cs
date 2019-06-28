using System;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class UserClaimEntity
    {
        private readonly DbUserClaim _claim;

        public UserClaimEntity(DbUserClaim claim)
        {
            _claim = claim;
        }

        public string ClaimType => _claim.ClaimType;

        public string ClaimValue => _claim.ClaimValue;

        public DateTime ClaimUpdatedDate => _claim.ClaimUpdatedDate;
    }
}