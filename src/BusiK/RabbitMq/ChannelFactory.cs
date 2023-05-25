using RabbitMQ.Client;

namespace BusiK.RabbitMq;

class ChannelFactory : IChannelFactory
{
    private readonly IConnection _connection;
    private readonly ChannelAccessor _channelAccessor;

    public ChannelFactory(IConnection connection, ChannelAccessor channelAccessor)
    {
        _connection = connection;
        _channelAccessor = channelAccessor;
    }
    
    public IModel Create()
    {
        if (_channelAccessor.Channel is not null)
        {
            return _channelAccessor.Channel;
        }
        
        _channelAccessor.Channel = _connection.CreateModel();
        return _channelAccessor.Channel;
    }
}
