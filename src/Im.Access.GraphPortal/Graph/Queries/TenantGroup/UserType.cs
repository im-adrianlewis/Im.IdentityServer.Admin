using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.Queries.TenantGroup
{
    public class UserType : ObjectGraphType<UserEntity>
    {
        public UserType()
        {
            Name = "User";
            Description = "Represents a single user.";

            Field(u => u.Id).Description("User identifier");
            Field(u => u.TenantId).Description("Tenant associated with the user account");
            Field(u => u.FirstName, true).Description("User's first name");
            Field(u => u.LastName, true).Description("User's last name");
            Field(u => u.Email, true).Description("User's email address");
            Field(u => u.EmailConfirmed).Description("Flag indicating whether email is confirmed");
            Field(u => u.PhoneNumber, true).Description("User's phone number");
            Field(u => u.PhoneNumberConfirmed).Description("Flag indicating whether phone number is confirmed");
            Field(u => u.LockoutEndDateUtc, true).Description("When set it is the date/time when the account will be unlocked");
            Field(u => u.LockoutEnabled).Description("Flag indicating whether account lockout has been enabled");
            Field(u => u.AccessFailedCount).Description("Number of consecutive sign-in failures");
            Field(u => u.UserName).Description("User name");
            Field(u => u.CreateDate).Description("Date when user account was created");
            Field(u => u.LastUpdatedDate).Description("Date when user account was last updated");
            Field(u => u.RegistrationDate, true).Description("Date when user account transitioned into registered account");
            Field(u => u.LastLoggedInDate, true).Description("Date when user account last logged in");
            Field(u => u.RegistrationIPAddress).Description("IP address where user account was registered");
            Field(u => u.LastLoggedInIPAddress).Description("IP address where user last logged in from");
            Field(u => u.ScreenName).Description("Forum screen name");
            Field(u => u.UserType).Description("User type");
            Field(u => u.Address1).Description("First line of the address");
            Field(u => u.Address2).Description("Second line of the address");
            Field(u => u.City).Description("City of residence");
            Field(u => u.County).Description("County of residence");
            Field(u => u.Country).Description("Country of residence");
            Field(u => u.Postcode).Description("Postal code");
            Field(u => u.UserBiography).Description("User biography");
            Field(u => u.FirstPartyIM).Description("Flag indicating whether user has opted-in for first-party mails");
            Field(u => u.FirstPartyImUpdatedDate).Description("Date when first-party opt-in flag was last changed");
            Field(u => u.AuthenticationType).Description("Authentication mechanism used when user was registered");
            Field<ListGraphType<UserClaimType>>("Claims", "User's claims");
        }
    }
}