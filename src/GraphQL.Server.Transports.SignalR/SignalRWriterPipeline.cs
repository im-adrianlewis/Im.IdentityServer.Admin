using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Newtonsoft.Json;

namespace GraphQL.Server.Transports.SignalR
{
    public class SignalRWriterPipeline : IWriterPipeline
    {
        private readonly ChannelWriter<string> _channelWriter;
        private readonly CancellationToken _cancellationToken;
        private readonly ITargetBlock<OperationMessage> _startBlock;

        public SignalRWriterPipeline(ChannelWriter<string> channelWriter, CancellationToken cancellationToken)
        {
            _channelWriter = channelWriter;
            _cancellationToken = cancellationToken;
            _startBlock = new ActionBlock<OperationMessage>(
                ProcessMessageAsync,
                new ExecutionDataflowBlockOptions
                {
                    BoundedCapacity = 1,
                    MaxDegreeOfParallelism = 1,
                    EnsureOrdered = true
                });
        }

        public bool Post(OperationMessage message)
        {
            return _startBlock.Post(message);
        }

        public Task SendAsync(OperationMessage message)
        {
            return _startBlock.SendAsync(message);
        }

        public Task Complete()
        {
            _startBlock.Complete();
            return Task.CompletedTask;
        }

        public Task Completion => _startBlock.Completion;

        private async Task ProcessMessageAsync(OperationMessage message)
        {
            try
            {
                var payload = JsonConvert.SerializeObject(message);
                if (await _channelWriter.WaitToWriteAsync())
                {
                    await _channelWriter.WriteAsync(payload);
                }
            }
            catch
            {
            }
        }
    }
}