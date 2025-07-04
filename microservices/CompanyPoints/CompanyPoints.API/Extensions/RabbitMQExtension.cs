using BuildingBlocks.Messaging.Extensions;
using Infrastructure.Consumers;

namespace CompanyPoints.API.Extensions;

internal static class RabbitMQExtension
{
    internal static void ConfigureRabbit(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddRabbitMqBus(
            configuration,
            cfg =>
            {
                cfg.AddConsumer<CompanyPointInfoConsumer>();
            });
    }
}
