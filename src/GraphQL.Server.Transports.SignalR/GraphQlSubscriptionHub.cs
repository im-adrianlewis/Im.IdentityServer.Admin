using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GraphQL.Server.Transports.SignalR
{
    public abstract class GraphQlSubscriptionHub : Hub<IGraphSubscription>
    {
        protected GraphQlSubscriptionHub()
        {
        }

        public abstract Task<ChannelWriter<string>> Execute(
            ChannelReader<string> stream,
            CancellationToken cancellationToken);
    }
}