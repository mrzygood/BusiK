namespace BusiK.Subscribers;

public interface IMessageSubscriber
{
    Task SubscribeAsync(string queue);
}
