
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using OrderHistory.API.Extensions;
using Application.Features.OrderChanges.Commands.PointChange;

namespace OrderHistory.API.Services.App
{
    public static class ConfigureService
    {
        public static void ConfigureHosting(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();

            services.ConfigureCors(configuration);
            services.ConfigureContextNpgsql(configuration);
            //services.ConfigureRedis(configuration);
            services.ConfigureRabbit(configuration);
            services.AddMediatR(cfg =>
         cfg.RegisterServicesFromAssembly(typeof(OrderPointChangeCommand).Assembly));

            services.AddScoped<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IOrderStatusChangeRepository, OrderStatusChangeRepository>();
            services.AddScoped<IOrderPointChangeRepository, OrderPointChangeRepository>();


            services.AddControllers();

            services.AddLogging();

            services.AddTransient<GlobalExceptionMiddleware>();

            services.Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

      
        }
    }
}
