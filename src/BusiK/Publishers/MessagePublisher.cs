using System.Text;
using System.Text.Json;
using BusiK.RabbitMq;
using BusiK.ValueObjects;
using RabbitMQ.Client;

namespace BusiK.Publishers;

public class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;

    public MessagePublisher(IChannelFactory channelFactory)
    {
        _channel = channelFactory.Create();
    }
    
    public Task PublishAsync<TMessage>(TMessage message) where TMessage : class
    {
        var messageType = message.GetType();
        var messageTypeUrn = Urn.FromMessageType(messageType);
        var messageEnvelope = new MessageEnvelope<TMessage>(message, messageTypeUrn);
        
        var exchangeName = MessageExchangeName.FromType(messageType);
        
        var messageProperties = _channel.CreateBasicProperties();

        var messageJson = JsonSerializer.Serialize(messageEnvelope);
        var body = Encoding.UTF8.GetBytes(messageJson);
        
        _channel.BasicPublish(
            exchangeName.Value,
            routingKey: string.Empty,
            basicProperties: messageProperties,
            body: body);

        return Task.CompletedTask;
    }
}
