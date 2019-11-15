using System;
using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.UserGroup.Queries
{
    public class UserSearchCriteriaType : InputObjectGraphType<UserSearchCriteria>
    {
        public UserSearchCriteriaType()
        {
            Name = "UserSearchCriteria";
            Description = "User search criteria used to filter result-set.";

            Field(c => c.TenantId, true)
                .Description("Limit search to specified tenant");
            Field(c => c.FirstName, true)
                .Description("First name search pattern");
            Field(c => c.LastName, true)
                .Description("Last name search pattern");
            Field(c => c.Email, true)
                .Description("Email search pattern");
            Field(c => c.ScreenName, true)
                .Description("Screen name search pattern");
            Field<DateTime?>(c => c.CreateDateFrom, true)
                .Description("Starting create date filter");
            Field<DateTime?>(c => c.CreateDateTo, true)
                .Description("Ending create date filter");
            Field<DateTime?>(c => c.LastLoggedInDateFrom, true)
                .Description("Starting last logged in date filter");
            Field<DateTime?>(c => c.LastLoggedInDateTo, true)
                .Description("Ending last logged in date filter");
            Field(c => c.PageIndex)
                .Description("Zero-based page index")
                .DefaultValue(0);
            Field(c => c.PageSize)
                .Description("Page size (default = 100)")
                .DefaultValue(100);
        }
    }
}