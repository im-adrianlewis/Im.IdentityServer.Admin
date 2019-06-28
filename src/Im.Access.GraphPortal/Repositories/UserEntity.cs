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

        public ICollection<UserClaimEntity> Claims => _user
            .Claims
            .Select(c => new UserClaimEntity(c))
            .ToList()
            .AsReadOnly();
    }
}