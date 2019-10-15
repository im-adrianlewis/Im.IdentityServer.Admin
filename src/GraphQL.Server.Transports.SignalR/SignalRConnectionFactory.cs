using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using GraphQL.Http;
using GraphQL.Server.Internal;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using GraphQL.Types;
using Microsoft.Extensions.Logging;

namespace GraphQL.Server.Transports.SignalR
{
    public class SignalRConnectionFactory<TSchema> : ISignalRConnectionFactory<TSchema>
        where TSchema : ISchema
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IGraphQLExecuter<TSchema> _executer;
        private readonly IEnumerable<IOperationMessageListener> _messageListeners;
        private readonly IDocumentWriter _documentWriter;

        public SignalRConnectionFactory(
            ILogger<SignalRConnectionFactory<TSchema>> logger,
            ILoggerFactory loggerFactory,
            IGraphQLExecuter<TSchema> executer,
            IEnumerable<IOperationMessageListener> messageListeners,
            IDocumentWriter documentWriter)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _executer = executer;
            _messageListeners = messageListeners;
            _documentWriter = documentWriter;
        }

        public SignalRConnection CreateConnection(
            GraphQlSubscriptionHub socket,
            string connectionId,
            ChannelReader<string> channelReader,
            ChannelWriter<string> channelWriter,
            CancellationToken cancellationToken)
        {
            this._logger.LogDebug("Creating server for connection {connectionId}", (object)connectionId);
            var transport = new GraphQlTransport(
                socket,
                channelReader,
                channelWriter,
                _documentWriter,
                cancellationToken);
            return new SignalRConnection(
                transport,
                new SubscriptionServer(
                    transport,
                    new SubscriptionManager(_executer, _loggerFactory),
                    _messageListeners,
                    _loggerFactory.CreateLogger<SubscriptionServer>()),
                cancellationToken);
        }
    }
}