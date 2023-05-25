using BusiK.ValueObjects;

namespace BusiK.MessageBroker;

public record ExchangeEntry(MessageExchangeName ExchangeName, Type MessageType);

public interface IBrokerStructureStore
{
    IDictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>> GetStructure();
}
