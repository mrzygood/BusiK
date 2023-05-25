namespace BusiK.Publishers;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : class;
}
