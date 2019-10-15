using GraphQL.Server.Transports.SignalR;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace GraphQL.Server
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGraphQLSignalR<TSchema>(
            this IApplicationBuilder builder,
            string path)
        {
            return builder.UseGraphQLSignalR<TSchema>(new PathString(path));
        }

        public static IApplicationBuilder UseGraphQLSignalR<TSchema>(
            this IApplicationBuilder builder,
            PathString path)
            where TSchema : ISchema
        {
            builder.UseEndpoints(
                    endpoints =>
                    {
                        endpoints.MapHub<GraphQLSubscriptionHub<TSchema>>(path);
                    });
            return builder.UseMiddleware<GraphQlSignalRMiddleware<TSchema>>(path);
        }
    }
}