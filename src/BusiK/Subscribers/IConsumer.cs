namespace BusiK.Subscribers;

public interface IConsumer
{
}

public interface IConsumer<TMessage> : IConsumer where TMessage : class
{
    Task ConsumeAsync(IConsumeContext<TMessage> message);
}
