using System.Reflection;
using System.Text.Json;
using BusiK.MessageBroker;
using BusiK.Publishers;
using BusiK.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace BusiK.Subscribers;

internal class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBrokerStructureStore _brokerStructureStore;
    
    public MessageDispatcher(IServiceScopeFactory serviceScopeFactory, IBrokerStructureStore brokerStructureStore)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _brokerStructureStore = brokerStructureStore;
    }
    
    public async Task DispatchAsync(string messageJson, string queueName)
    {
        await ExecuteConsumerOfMessageType(messageJson, queueName);
    }

    private async Task ExecuteConsumerOfMessageType(string messageJson, string queueName)
    {
        var messageType = GetTypeOfMessageBeingProcessed(messageJson, queueName);

        var consumerClassName = ConsumerExchangeAndQueueName.FromQueueName(queueName).ToConsumerClassName();
        
        Type consumerRawGenericType = typeof(IConsumer<>);
        Type messageConsumerType = consumerRawGenericType.MakeGenericType(messageType);

        var messageConsumer = GetConsumerFromIocContainer(messageConsumerType, consumerClassName);
        
        var consumeContext = CreateConsumerMessageContext(messageJson, messageType);

        var consumerTask = (Task)messageConsumerType.InvokeMember("ConsumeAsync",
            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
            null, messageConsumer, new object[] { consumeContext })!;

        await consumerTask;
    }

    private IConsumer GetConsumerFromIocContainer(Type messageConsumerType, string consumerClassName)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var messageConsumers = scope.ServiceProvider.GetServices(messageConsumerType);
        return messageConsumers.First(x => x != null && x.GetType().Name == consumerClassName) as IConsumer;
    }

    private Type GetTypeOfMessageBeingProcessed(string messageJson, string queueName)
    {
        var baseMessageEnvelope = JsonSerializer.Deserialize<MessageUrnEnvelope>(messageJson)!;

        var messageTypeFullName = new Urn(baseMessageEnvelope.Urn).GetMessageTypeFullName();

        var consumerConsumingMessageType = _brokerStructureStore
            .GetStructure()
            .SingleOrDefault(x =>
                x.Key.Value == queueName &&
                x.Value.Any(m => m.MessageType.FullName == messageTypeFullName));

        return consumerConsumingMessageType
            .Value
            .Where(x => x.MessageType.FullName == messageTypeFullName)
            .Select(x => x.MessageType)
            .Single();
    }

    private IConsumeContext CreateConsumerMessageContext(string messageJson, Type messageType)
    {
        Type messageEnvelopeGenericType = typeof(MessageEnvelope<>);
        Type messageEnvelopeExactType = messageEnvelopeGenericType.MakeGenericType(messageType);

        dynamic messageEnvelope = JsonSerializer.Deserialize(messageJson, messageEnvelopeExactType);
        
        Type genericConsumerClass = typeof(ConsumeContext<>);
        
        Type specificConsumerContext = genericConsumerClass.MakeGenericType(messageType);
        
        ConstructorInfo constructor = specificConsumerContext.GetConstructor(new [] { messageType });

        return constructor.Invoke(new [] { messageEnvelope.Message }) as IConsumeContext;
    }
}
