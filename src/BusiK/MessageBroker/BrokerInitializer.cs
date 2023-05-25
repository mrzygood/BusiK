using BusiK.RabbitMq;
using BusiK.ValueObjects;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BusiK.MessageBroker;

public sealed class BrokerInitializer : IHostedService
{
    private readonly IModel _channel;
    private readonly IBrokerStructureStore _brokerStructureStore;
    
    public BrokerInitializer(IChannelFactory channelFactory, IBrokerStructureStore brokerStructureStore)
    {
        _brokerStructureStore = brokerStructureStore;
        _channel = channelFactory.Create();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var brokerStructureConfig = _brokerStructureStore.GetStructure();

        DeclareMessagesExchanges(brokerStructureConfig);

        DeclareConsumersExchangesAndBindToQueues(brokerStructureConfig);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void DeclareConsumersExchangesAndBindToQueues(IDictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>> brokerStructureConfig)
    {
        foreach (var consumerConfig in brokerStructureConfig)
        {
            var consumerExchangeName = consumerConfig.Key.Value;
            _channel.ExchangeDeclare(consumerExchangeName, ExchangeType.Fanout);

            foreach (var consumerBoundedMessageExchange in consumerConfig.Value)
            {
                _channel.ExchangeBind(consumerExchangeName, consumerBoundedMessageExchange.ExchangeName.Value, string.Empty);
            }

            _channel.QueueDeclare(consumerExchangeName, autoDelete: false, exclusive: false);
            _channel.QueueBind(consumerExchangeName, consumerExchangeName, string.Empty);
        }
    }

    private void DeclareMessagesExchanges(IDictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>> brokerStructureConfig)
    {
        var messagesExchanges = new List<MessageExchangeName>();
        foreach (var consumerMessageExchanges in brokerStructureConfig.Values)
        {
            messagesExchanges = messagesExchanges
                .Concat(consumerMessageExchanges.Select(x => x.ExchangeName))
                .ToList();
        }

        foreach (var messageExchangeName in messagesExchanges.Distinct())
        {
            _channel.ExchangeDeclare(messageExchangeName.Value, ExchangeType.Fanout);
        }
    }
}
