using Ecommerce.Messages;
using MassTransit;

namespace Ecommerce.Monolith.MassTransit.Consumers;

public sealed class ProductModificationConsumer : IConsumer<ProductRemoved>, IConsumer<ProductAdded>
{
    public Task Consume(ConsumeContext<ProductRemoved> message)
    {
        Console.WriteLine("MassTransit > ProductModificationConsumer:ProductRemoved");
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<ProductAdded> message)
    {
        Console.WriteLine("MassTransit > ProductModificationConsumer:ProductAdded");
        return Task.CompletedTask;
    }
}
