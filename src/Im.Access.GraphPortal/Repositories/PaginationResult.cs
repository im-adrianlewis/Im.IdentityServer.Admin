using System.Collections.Generic;

namespace Im.Access.GraphPortal.Repositories
{
    public class PaginationResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}