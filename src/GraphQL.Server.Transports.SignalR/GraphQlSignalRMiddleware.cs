using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GraphQL.Server.Transports.SignalR
{
    public class GraphQlSignalRMiddleware<TSchema> where TSchema : ISchema
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly ILogger<GraphQlSignalRMiddleware<TSchema>> _logger;

        public GraphQlSignalRMiddleware(
            RequestDelegate next,
            PathString path,
            ILogger<GraphQlSignalRMiddleware<TSchema>> logger)
        {
            _next = next;
            _path = path;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.BeginScope(
                new Dictionary<string, object>
                {
                    ["ConnectionId"] = (object)context.Connection.Id,
                    ["Request"] = (object)context.Request
                }))
            {
                // We don't need a hub instance to handle sending messages
                var hubContext = context.RequestServices.GetRequiredService<IHubContext<GraphQlSubscriptionHub>>();

                if (!context.WebSockets.IsWebSocketRequest || !context.Request.Path.StartsWithSegments(this._path))
                {
                    this._logger.LogDebug("Request is not a valid websocket request");
                    await this._next(context);
                    return;
                }
                this._logger.LogDebug("Connection is a valid websocket request");
                //WebSocket socket = await context.WebSockets.AcceptWebSocketAsync("graphql-ws").ConfigureAwait(false);
                //if (!context.WebSockets.WebSocketRequestedProtocols.Contains(socket.SubProtocol))
                //{
                //    this._logger.LogError("Websocket connection does not have correct protocol: graphql-ws. Request protocols: {protocols}", (object)context.WebSockets.WebSocketRequestedProtocols);
                //    await socket.CloseAsync(WebSocketCloseStatus.ProtocolError, "Server only supports graphql-ws protocol", context.RequestAborted).ConfigureAwait(false);
                //    return;
                //}

                //using (this._logger.BeginScope<string>(string.Format("GraphQL websocket connection: {0}",
                //    (object) context.Connection.Id)))
                //{
                //    await context.RequestServices.GetRequiredService<IWebSocketConnectionFactory<TSchema>>().CreateConnection(socket, context.Connection.Id).Connect();
                //}
            }
        }
    }

    public class GraphQLSubscriptionHub<TSchema> : GraphQlSubscriptionHub where TSchema : ISchema
    {
        private readonly ISignalRConnectionFactory<TSchema> _connectionFactory;

        public GraphQLSubscriptionHub(ISignalRConnectionFactory<TSchema> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public override async Task<ChannelWriter<string>> Execute(
            ChannelReader<string> stream,
            CancellationToken cancellationToken)
        {
            var channelWriter = Channel.CreateUnbounded<string>();
            var connection = _connectionFactory
                .CreateConnection(
                    this,
                    Context.ConnectionId,
                    stream,
                    channelWriter,
                    cancellationToken);
            await connection.Connect();
            return channelWriter;
        }
    }
}
