using System.Reflection;
using MassTransit;

namespace Ecommerce.Monolith.MassTransit;

public static class AddMassTransitExtension
{
    public static IServiceCollection AddMassTransitIntegration(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assemblies);
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
            
        return services;
    } 
}
