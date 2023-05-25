namespace BusiK.ValueObjects;

public sealed record MessageExchangeName
{
    public static MessageExchangeName FromType(Type messageType)
    {
        return new MessageExchangeName($"{messageType.Namespace}:{messageType.Name}");
    }

    private MessageExchangeName(string exchangeName)
    {
        Value = exchangeName;
    }
    
    public string Value { get; }
}
