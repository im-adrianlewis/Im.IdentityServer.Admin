using System;

namespace Im.Access.GraphPortal.Data
{
    public class DbUserClaim
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public DateTime ClaimUpdatedDate { get; set; }
    }
}
