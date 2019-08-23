using System;
using System.Collections.Generic;
using System.Linq;
using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class UserEntity
    {
        private readonly DbUser _user;

        public UserEntity(DbUser user)
        {
            _user = user;
        }

        public string Id => _user.Id;

        public string TenantId => _user.TenantId;

        public string FirstName => _user.FirstName;

        public string LastName => _user.LastName;

        public string Email => _user.Email;

        public bool EmailConfirmed => _user.EmailConfirmed;

        public string PhoneNumber => _user.PhoneNumber;

        public bool PhoneNumberConfirmed => _user.PhoneNumberConfirmed;

        public DateTime? LockoutEndDateUtc => _user.LockoutEndDateUtc;

        public bool LockoutEnabled => _user.LockoutEnabled;

        public int AccessFailedCount => _user.AccessFailedCount;

        public string UserName => _user.UserName;

        public DateTime? RegistrationDate => _user.RegistrationDate;

        public DateTime? LastLoggedInDate => _user.RegistrationDate;

        public string RegistrationIPAddress => _user.RegistrationIPAddress;

        public string LastLoggedInIPAddress => _user.LastLoggedInIPAddress;

        public string ScreenName => _user.ScreenName;

        public DateTime CreateDate => _user.CreateDate;

        public DateTime LastUpdatedDate => _user.LastUpdatedDate;

        public string UserType => _user.UserType;

        public string Address1 => _user.Address1;

        public string Address2 => _user.Address2;

        public string City => _user.City;

        public string County => _user.County;

        public string Country => _user.Country;

        public string Postcode => _user.Postcode;

        public string UserBiography => _user.UserBiography;

        public bool FirstPartyIM => _user.FirstPartyIM;

        public DateTime FirstPartyImUpdatedDate => _user.FirstPartyImUpdatedDate;

        public string AuthenticationType => _user.AuthenticationType;

        public ICollection<UserClaimEntity> Claims => _user
            .Claims
            .Select(c => new UserClaimEntity(c))
            .ToList()
            .AsReadOnly();
    }
}