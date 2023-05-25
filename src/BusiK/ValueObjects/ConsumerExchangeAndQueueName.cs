using Humanizer;

namespace BusiK.ValueObjects;

public sealed record ConsumerExchangeAndQueueName
{
    public static ConsumerExchangeAndQueueName FromConsumerClassName(string className)
    {
        return new ConsumerExchangeAndQueueName(
            className.Replace("Consumer", string.Empty).Kebaberize());
    }
    
    public static ConsumerExchangeAndQueueName FromQueueName(string queueName)
    {
        return new ConsumerExchangeAndQueueName(queueName);
    }

    private ConsumerExchangeAndQueueName(string exchangeName)
    {
        Value = exchangeName;
    }

    public string ToConsumerClassName() => Value.Replace("-", "_").Pascalize() + "Consumer";
    
    public string Value { get; }
}
