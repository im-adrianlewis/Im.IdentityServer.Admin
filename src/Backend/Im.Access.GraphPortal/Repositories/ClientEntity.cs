using Im.Access.GraphPortal.Data;

namespace Im.Access.GraphPortal.Repositories
{
    public class ClientEntity
    {
        private readonly DbClient _client;

        public ClientEntity(DbClient client)
        {
            _client = client;
        }

        public int Id => _client.Id;

        public bool Enabled => _client.Enabled;

        public string ClientId => _client.ClientId;

        public string ClientName => _client.ClientName;

        public string ClientUri => _client.ClientUri;

        public string LogoUri => _client.LogoUri;

        public bool RequireConsent => _client.RequireConsent;

        public bool AllowRequireConsent => _client.AllowRequireConsent;

        public bool AllowAccessTokensViaBrowser => _client.AllowAccessTokensViaBrowser;

        public int Flow => _client.Flow;

        public bool AllowClientCredentialsOnly => _client.AllowClientCredentialsOnly;

        public string LogoutUri => _client.LogoutUri;

        public bool LogoutSessionRequired => _client.LogoutSessionRequired;

        public bool RequireSignOutPrompt => _client.RequireSignOutPrompt;

        public bool AllowAccessToAllScopes => _client.AllowAccessToAllScopes;

        public int IdentityTokenLifetime => _client.IdentityTokenLifetime;

        public int AccessTokenLifetime => _client.AccessTokenLifetime;

        public int AuthorizationCodeLifetime => _client.AuthorizationCodeLifetime;

        public int SlidingRefreshTokenLifetime => _client.SlidingRefreshTokenLifetime;

        public int RefreshTokenUsage => _client.RefreshTokenUsage;

        public bool UpdateAccessTokenOnRefresh => _client.UpdateAccessTokenOnRefresh;

        public int RefreshTokenExpiration => _client.RefreshTokenExpiration;

        public int AccessTokenType => _client.AccessTokenType;

        public bool EnableLocalLogin => _client.EnableLocalLogin;

        public bool IncludeJwtId => _client.IncludeJwtId;

        public bool AlwaysSendClientClaims => _client.AlwaysSendClientClaims;

        public bool PrefixClientClaims => _client.PrefixClientClaims;

        public bool AllowAccessToAllGrantTypes => _client.AllowAccessToAllGrantTypes;
    }
}