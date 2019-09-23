namespace Im.Access.GraphPortal.Repositories
{
    public class ClientSearchCriteria
    {
        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}