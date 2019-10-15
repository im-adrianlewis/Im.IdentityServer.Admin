using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Newtonsoft.Json;

namespace GraphQL.Server.Transports.SignalR
{
    public class GraphQlTransport : IMessageTransport
    {
        private readonly GraphQlSubscriptionHub _hub;
        private readonly IDocumentWriter _documentWriter;

        public GraphQlTransport(
            GraphQlSubscriptionHub hub,
            ChannelReader<string> channelReader,
            ChannelWriter<string> channelWriter,
            IDocumentWriter documentWriter,
            CancellationToken cancellationToken)
        {
            _hub = hub;
            _documentWriter = documentWriter;

            Reader = new SignalRReaderPipeline(
                channelReader,
                cancellationToken,
                new JsonSerializerSettings());
            Writer = new SignalRWriterPipeline(
                channelWriter,
                cancellationToken);
        }

        public IReaderPipeline Reader { get; }

        public IWriterPipeline Writer { get; }

        public Task CloseAsync()
        {
            _hub.Dispose();
            return Task.CompletedTask;
        }
    }
}