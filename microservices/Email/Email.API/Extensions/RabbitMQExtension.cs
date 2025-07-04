using BuildingBlocks.Messaging.Extensions;
using Infrastructure.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email.API.Extensions;

internal static class RabbitMQExtension
{
    internal static void ConfigureRabbit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqBus(
            configuration,
            cfg =>
            {
                cfg.AddConsumer<EmailSendingConsumer>();
            });
    }
}
