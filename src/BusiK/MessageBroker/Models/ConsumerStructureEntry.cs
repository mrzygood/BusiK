namespace BusiK.MessageBroker.Models;

public sealed record ConsumerStructureEntry(string ConsumerClassName, ICollection<Type> Messages);
