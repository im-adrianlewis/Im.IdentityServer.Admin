using System.Threading;
using System.Threading.Tasks;
using GraphQL.Server.Transports.Subscriptions.Abstractions;

namespace GraphQL.Server.Transports.SignalR
{
    public class SignalRConnection
    {
        private readonly GraphQlTransport _transport;
        private readonly SubscriptionServer _subscriptionServer;
        private readonly CancellationToken _cancellationToken;

        public SignalRConnection(
            GraphQlTransport transport,
            SubscriptionServer subscriptionServer,
            CancellationToken cancellationToken)
        {
            _transport = transport;
            _subscriptionServer = subscriptionServer;
            _cancellationToken = cancellationToken;
        }

        public async Task Connect()
        {
            await _subscriptionServer.OnConnect();

            _cancellationToken.Register(
                () => Close().GetAwaiter().GetResult());
        }

        public async Task Close()
        {
            await _subscriptionServer.OnDisconnect();
            await _transport.CloseAsync();
        }
    }
}