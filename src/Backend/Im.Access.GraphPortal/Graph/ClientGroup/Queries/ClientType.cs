using GraphQL.Types;
using Im.Access.GraphPortal.Repositories;

namespace Im.Access.GraphPortal.Graph.ClientGroup.Queries
{
    public class ClientType : ObjectGraphType<ClientEntity>
    {
        public ClientType()
        {
            Field(c => c.Id);
            Field(c => c.ClientName);
            Field(c => c.ClientId);
            Field(c => c.ClientUri);
            Field(c => c.AccessTokenLifetime);
            Field(c => c.AccessTokenType);
            Field(c => c.AllowAccessToAllGrantTypes);
            Field(c => c.AllowAccessToAllScopes);
            Field(c => c.AllowAccessTokensViaBrowser);
            Field(c => c.AllowClientCredentialsOnly);
            Field(c => c.AllowRequireConsent);
            Field(c => c.AlwaysSendClientClaims);
            Field(c => c.AuthorizationCodeLifetime);
            Field(c => c.EnableLocalLogin);
            Field(c => c.Enabled);
            Field(c => c.Flow);
            Field(c => c.IdentityTokenLifetime);
            Field(c => c.IncludeJwtId);
            Field(c => c.LogoUri);
            Field(c => c.LogoutSessionRequired);
            Field(c => c.LogoutUri);
            Field(c => c.PrefixClientClaims);
            Field(c => c.RefreshTokenExpiration);
            Field(c => c.RefreshTokenUsage);
            Field(c => c.RequireConsent);
            Field(c => c.RequireSignOutPrompt);
            Field(c => c.SlidingRefreshTokenLifetime);
            Field(c => c.UpdateAccessTokenOnRefresh);
        }
    }
}