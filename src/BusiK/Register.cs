using BusiK.MessageBroker;
using BusiK.Publishers;
using BusiK.RabbitMq;
using BusiK.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BusiK;

public static class Register
{
    public static IServiceCollection AddBusiK(this IServiceCollection services, Action<IBusiKConfiguration>? configure = default)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        
        services.AddSingleton(connection);
        services.AddSingleton<IConsumersStructureStore, ConsumersStructureStore>();
        services.AddHostedService<BrokerInitializer>();
        services.AddHostedService<MessageSubscribersAutoRegistration>();
        services.AddSingleton<IChannelFactory, ChannelFactory>();
        services.AddSingleton<ChannelAccessor>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
        services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        services.AddSingleton<IBrokerStructureStore, BrokerStructureStore>();
        
        configure?.Invoke(new BusiKConfiguration(services));
        
        return services;
    }
}

public interface IBusiKConfiguration
{
}

internal sealed record BusiKConfiguration(IServiceCollection Services) : IBusiKConfiguration;

public static class ConsumersAutoRegistrationExtension
{
    public static IBusiKConfiguration AddConsumersAutoRegistration(this IBusiKConfiguration configuration, IServiceCollection services)
    {
        services.AddHostedService<MessageSubscribersAutoRegistration>();
        
        return configuration;
    }
}
