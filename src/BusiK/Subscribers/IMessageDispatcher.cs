namespace BusiK.Subscribers;

public interface IMessageDispatcher
{
    Task DispatchAsync(string messageJson, string queueName);
}
