using BusiK.MessageBroker;
using Humanizer;
using Microsoft.Extensions.Hosting;

namespace BusiK.Subscribers;

public sealed class MessageSubscribersAutoRegistration : BackgroundService
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly IConsumersStructureStore _consumersStructureStore;

    public MessageSubscribersAutoRegistration(IMessageSubscriber messageSubscriber, IConsumersStructureStore consumersStructureStore)
    {
        _messageSubscriber = messageSubscriber;
        _consumersStructureStore = consumersStructureStore;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerQueuesNamesByConvention = _consumersStructureStore
            .GetConfig()
            .Select(x => x.ConsumerClassName.Replace("Consumer", string.Empty).Kebaberize())
            .ToList();
        foreach (var consumerQueueName in consumerQueuesNamesByConvention)
        {
            await _messageSubscriber.SubscribeAsync(consumerQueueName);
        }
    }
}
