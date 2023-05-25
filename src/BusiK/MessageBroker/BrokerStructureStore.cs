using BusiK.MessageBroker.Models;
using BusiK.ValueObjects;
using Humanizer;

namespace BusiK.MessageBroker;

public class BrokerStructureStore : IBrokerStructureStore
{
    private readonly IConsumersStructureStore _consumersStructureStore;
    
    public BrokerStructureStore(IConsumersStructureStore consumersStructureStore)
    {
        _consumersStructureStore = consumersStructureStore;
    }

    public IDictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>> GetStructure()
    {
        return ToBrokerStructure(_consumersStructureStore.GetConfig());
    }

    private IDictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>> ToBrokerStructure(
        ICollection<ConsumerStructureEntry> consumersLog)
    {
        var result = new Dictionary<ConsumerExchangeAndQueueName, ICollection<ExchangeEntry>>();
        
        foreach (var consumerEntry in consumersLog)
        {
            var consumerMessagesExchanges = new List<ExchangeEntry>();
            foreach (var messageType in consumerEntry.Messages)
            {
                var exchangeName = MessageExchangeName.FromType(messageType);
                consumerMessagesExchanges.Add(new ExchangeEntry(exchangeName, messageType));
            }

            var consumerKey = ConsumerExchangeAndQueueName.FromConsumerClassName(consumerEntry.ConsumerClassName);
            
            if (result.ContainsKey(consumerKey))
            {
                result[consumerKey] = result[consumerKey].Concat(consumerMessagesExchanges).ToList();
            }
            else
            {
                result.Add(consumerKey, consumerMessagesExchanges);  
            }
        }
        
        return result;
    }
}
