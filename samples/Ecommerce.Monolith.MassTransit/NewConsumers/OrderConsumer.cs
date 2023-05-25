using Ecommerce.Messages;
using MassTransit;

namespace Ecommerce.Monolith.MassTransit.NewConsumers;

public sealed class OrderConsumer: IConsumer<ProductRemoved>
{
    public Task Consume(ConsumeContext<ProductRemoved> message)
    {
        Console.WriteLine("MassTransit > OrderConsumer2:ProductRemoved");
        return Task.CompletedTask;
    }
}
