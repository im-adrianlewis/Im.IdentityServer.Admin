using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.UserGroup
{
    public class PaginationType<TDestination, TSource> : ObjectGraphType<PaginationResult<TSource>>
        where TDestination : IGraphType
    {
        public PaginationType()
        {
            Name = "PagedResults";
            Description = "Paginated result-set used to return a sub-set/page of data.";

            Field(e => e.PageIndex).Description("Zero-based page index.");
            Field(e => e.PageSize).Description("Number of elements to fetch in each page.");
            Field(e => e.TotalCount).Description("Total count of items in query.");
            Field<ListGraphType<TDestination>>(
                "Items",
                "Page of actual data items.",
                resolve: e => e.Source.Items);
        }
    }
}