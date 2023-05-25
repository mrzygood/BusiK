using RabbitMQ.Client;

namespace BusiK.RabbitMq;

public interface IChannelFactory
{
    IModel Create();
}
