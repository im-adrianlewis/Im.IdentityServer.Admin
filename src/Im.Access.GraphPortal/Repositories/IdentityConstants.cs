namespace Im.Access.GraphPortal.Repositories
{
    public static class IdentityConstants
    {
        public static class Role
        {
            public static readonly string SuperAdministrator = "superadmin";
        }

        public static class Claim
        {
            public static readonly string Administrator = "admin"; // this claim needs the tenant id as a value
        }
    }
}