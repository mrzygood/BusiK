using BusiK.Subscribers;
using Ecommerce.Messages;

namespace Ecommerce.Monolith.Consumers;

public sealed class ProductModificationConsumer: IConsumer<ProductRemoved>, IConsumer<ProductAdded>
{
    public Task ConsumeAsync(IConsumeContext<ProductRemoved> message)
    {
        Console.WriteLine("ProductModificationConsumer:ProductRemoved");
        return Task.CompletedTask;
    }

    public Task ConsumeAsync(IConsumeContext<ProductAdded> message)
    {
        Console.WriteLine("ProductModificationConsumer:ProductAdded");
        return Task.CompletedTask;
    }
}
