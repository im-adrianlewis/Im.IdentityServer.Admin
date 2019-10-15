using System.Threading.Tasks;
using GraphQL.Server.Transports.Subscriptions.Abstractions;

namespace GraphQL.Server.Transports.SignalR
{
    public interface IGraphSubscription
    {
        Task ReceiveMessage(OperationMessage message);
    }
}