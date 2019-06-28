using System;
using System.Collections.Generic;

namespace Im.Access.GraphPortal.Data
{
    public class DbUser
    {
        public string Id { get; set; }

        public string TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public string UserName { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateTime? LastLoggedInDate { get; set; }

        public string RegistrationIPAddress { get; set; }

        public string LastLoggedInIPAddress { get; set; }

        public string ScreenName { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserType { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Postcode { get; set; }

        public string UserBiography { get; set; }

        public bool FirstPartyIM { get; set; }

        public string County { get; set; }

        public DateTime FirstPartyImUpdatedDate { get; set; }

        public string AuthenticationType { get; set; }

        public ICollection<DbUserClaim> Claims { get; set; }
    }
}
