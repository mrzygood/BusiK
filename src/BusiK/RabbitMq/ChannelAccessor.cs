using RabbitMQ.Client;

namespace BusiK.RabbitMq;

// Inspired by HttpContextAccessor
// https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/HttpContextAccessor.cs
public class ChannelAccessor
{
    private static readonly ThreadLocal<ChannelHolder> Holder = new ();

    public IModel? Channel
    {
        get => Holder.Value?.Channel;
        set
        {
            var holder = Holder.Value;
            if (holder != null)
            {
                holder.Channel = null;
            }

            if (value is not null)
            {
                Holder.Value = new ChannelHolder { Channel = value };
            }
        }
    }

    private class ChannelHolder
    {
        public IModel? Channel;
    }
}
