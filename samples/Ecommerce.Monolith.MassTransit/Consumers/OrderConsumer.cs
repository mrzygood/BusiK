using Ecommerce.Messages;
using MassTransit;

namespace Ecommerce.Monolith.MassTransit.Consumers;

public sealed class OrderConsumer: IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> message)
    {
        Console.WriteLine("MassTransit > OrderConsumer:OrderPlaced");
        return Task.CompletedTask;
    }
}
