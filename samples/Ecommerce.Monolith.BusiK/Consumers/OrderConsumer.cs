using BusiK.Subscribers;
using Ecommerce.Messages;

namespace Ecommerce.Monolith.Consumers;

public sealed class OrderConsumer: IConsumer<OrderPlaced>
{
    public Task ConsumeAsync(IConsumeContext<OrderPlaced> message)
    {
        Console.WriteLine("OrderConsumer:OrderPlaced");
        return Task.CompletedTask;
    }
}
