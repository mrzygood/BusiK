using System.Reflection;
using BusiK.MessageBroker.Models;
using BusiK.Subscribers;

namespace BusiK.MessageBroker;

public interface IConsumersStructureStore
{
    ICollection<ConsumerStructureEntry> GetConfig();
}

public class ConsumersStructureStore : IConsumersStructureStore
{
    private ICollection<ConsumerStructureEntry>? _config;
    
    public ICollection<ConsumerStructureEntry> GetConfig()
    {
        return _config ??= FetchConsumersWithRelatedMessages();
    }
    
    private static IList<ConsumerStructureEntry> FetchConsumersWithRelatedMessages()
    {
        var consumerInterface = typeof(IConsumer<>);

        var consumerTypes = GetAssemblies()
            .GetTypesImplementingGenericInterface(consumerInterface)
            .Select(x => new ConsumerStructureEntry(x.Name, x.GetInterfaceGenericArgumentsForClass(consumerInterface)))
            .ToList();

        return consumerTypes;
    }

    // TODO user have to specify assemblies as params
    private static IEnumerable<Assembly> GetAssemblies()
    {
        /* VERSION 1 */
        // var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.Contains("Ecommerce."));
        
        /* VERSION 2 */
        var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => x.Contains("Ecommerce."))
            .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
        
        // Get all loaded assemblies
        var loadedAssemblies = assemblies
            .Where(x => x.GetName().Name != null && x.GetName().Name!.Contains("Ecommerce."));

        return loadedAssemblies;
    }
}
