using BusiK.Subscribers;
using Ecommerce.Messages;

namespace Ecommerce.Monolith.NewConsumers;

public sealed class OrderConsumer: IConsumer<ProductRemoved>
{
    public Task ConsumeAsync(IConsumeContext<ProductRemoved> message)
    {
        Console.WriteLine("MassTransit > OrderConsumer2:ProductRemoved");
        return Task.CompletedTask;
    }
}
