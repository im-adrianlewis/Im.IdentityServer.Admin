using System.Threading;
using System.Threading.Channels;
using GraphQL.Types;

namespace GraphQL.Server.Transports.SignalR
{
    public interface ISignalRConnectionFactory<TSchema> where TSchema : ISchema
    {
        SignalRConnection CreateConnection(
            GraphQlSubscriptionHub hub,
            string connectionId,
            ChannelReader<string> channelReader,
            ChannelWriter<string> channelWriter,
            CancellationToken cancellationToken);
    }
}