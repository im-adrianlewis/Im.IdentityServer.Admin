using System;

namespace Im.Access.GraphPortal.Repositories
{
    public class UserSearchCriteria
    {
        public string TenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ScreenName { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo { get; set; }

        public DateTime? LastLoggedInDateFrom { get; set; }

        public DateTime? LastLoggedInDateTo { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}