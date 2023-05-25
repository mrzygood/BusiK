using System.Reflection;

namespace BusiK.MessageBroker;

internal static class ReflectionExtension {
    public static IEnumerable<Type> GetTypesImplementingGenericInterface(
        this IEnumerable<Assembly> assemblies,
        Type genericInterfaceType)
    {
        return assemblies
            .SelectMany(s => s.GetTypes())
            .Where(x => x.GetInterfaces()
                .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == genericInterfaceType));
    }
    
    public static ICollection<Type> GetInterfaceGenericArgumentsForClass(
        this Type classImplementingInterface,
        Type genericInterfaceType)
    {
        return classImplementingInterface
            .GetInterfaces()
            .Where(y => y.IsGenericType && y.GetGenericTypeDefinition() == genericInterfaceType)
            .Select(i => i.GetGenericArguments()[0])
            .ToList();
    }
}
