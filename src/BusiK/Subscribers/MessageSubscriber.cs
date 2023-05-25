using System.Text;
using BusiK.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusiK.Subscribers;

public class MessageSubscriber : IMessageSubscriber
{
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly IModel _channel;
    
    public MessageSubscriber(IChannelFactory channelFactory, IMessageDispatcher messageDispatcher)
    {
        _messageDispatcher = messageDispatcher;
        _channel = channelFactory.Create();
    }
    
    public Task SubscribeAsync(string queue)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
            
            await _messageDispatcher.DispatchAsync(messageJson, queue);
            
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        
        _channel.BasicConsume(queue: queue,
            autoAck: false,
            consumer: consumer);
        
        return Task.CompletedTask;
    }
}
