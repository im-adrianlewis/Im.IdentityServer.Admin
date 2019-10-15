using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using Newtonsoft.Json;

namespace GraphQL.Server.Transports.SignalR
{
    public class SignalRReaderPipeline : IReaderPipeline
    {
        private readonly ChannelReader<string> _channelReader;
        private readonly CancellationToken _cancellationToken;
        private readonly IPropagatorBlock<string, string> _sourceBlock;
        private readonly IPropagatorBlock<string, OperationMessage> _endBlock;
        private readonly JsonSerializerSettings _serializerSettings;

        public SignalRReaderPipeline(
            ChannelReader<string> channelReader,
            CancellationToken cancellationToken,
            JsonSerializerSettings serializerSettings)
        {
            _channelReader = channelReader;
            _cancellationToken = cancellationToken;
            _serializerSettings = serializerSettings;

            _sourceBlock = new BufferBlock<string>(
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true,
                    BoundedCapacity = 1,
                    MaxDegreeOfParallelism = 1
                });
            _endBlock = new TransformBlock<string, OperationMessage>(
                (input) => JsonConvert.DeserializeObject<OperationMessage>(input, _serializerSettings),
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true
                });
            _sourceBlock.LinkTo(
                _endBlock,
                new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });

            Task.Run(ReadMessageAsync);
        }

        public void LinkTo(ITargetBlock<OperationMessage> target)
        {
            _endBlock.LinkTo(
                target,
                new DataflowLinkOptions
                {
                    PropagateCompletion = true
                });
        }

        public Task Complete()
        {
            try
            {
            }
            finally
            {
                _sourceBlock.Complete();
            }

            return Task.CompletedTask;
        }

        public Task Completion => _endBlock.Completion;

        private async Task ReadMessageAsync()
        {
            while (_cancellationToken.IsCancellationRequested)
            {
                if (await _channelReader.WaitToReadAsync(_cancellationToken))
                {
                    string payload = string.Empty;
                    try
                    { 
                        payload = await _channelReader.ReadAsync(_cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        _sourceBlock.Fault(exception);
                        continue;
                    }
                
                    await _sourceBlock.SendAsync(payload, _cancellationToken);
                }
            }
        }
    }
}