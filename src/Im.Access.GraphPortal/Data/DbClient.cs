using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Im.Access.GraphPortal.Data
{
    public class DbClient
    {
        [Key]
        public int Id { get; set; }

        public bool Enabled { get; set; }

        [StringLength(200)]
        public string ClientId { get; set; }

        [StringLength(200)]
        public string ClientName { get; set; }

        [StringLength(2000)]
        public string ClientUri { get; set; }

        [StringLength(250)]
        public string LogoUri { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowRequireConsent { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public int Flow { get; set; }

        public bool AllowClientCredentialsOnly { get; set; }

        [StringLength(250)]
        public string LogoutUri { get; set; }

        public bool LogoutSessionRequired { get; set; }

        public bool RequireSignOutPrompt { get; set; }

        public bool AllowAccessToAllScopes { get; set; }

        public int IdentityTokenLifetime { get; set; }

        public int AccessTokenLifetime { get; set; }

        public int AuthorizationCodeLifetime { get; set; }

        public int SlidingRefreshTokenLifetime { get; set; }

        public int RefreshTokenUsage { get; set; }

        public bool UpdateAccessTokenOnRefresh { get; set; }

        public int RefreshTokenExpiration { get; set; }

        public int AccessTokenType { get; set; }

        public bool EnableLocalLogin { get; set; }

        public bool IncludeJwtId { get; set; }

        public bool AlwaysSendClientClaims { get; set; }

        public bool PrefixClientClaims { get; set; }

        public bool AllowAccessToAllGrantTypes { get; set; }


        public ICollection<DbClientClaim> Claims { get; set; }
    }

    public class DbClientClaim
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Type { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public int ClientId { get; set; }
    }
}
