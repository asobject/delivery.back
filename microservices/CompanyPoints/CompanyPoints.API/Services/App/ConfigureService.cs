
using Domain.Interfaces.Repositories;
using Infrastructure.Repositories;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using BuildingBlocks.Interfaces.Repositories;
using CompanyPoints.API.Extensions;
using Application.Features.Points.Commands.CreatePoint;
using Domain.Entities;

namespace CompanyPoints.API.Services.App
{
    public static class ConfigureService
    {
        public static void ConfigureHosting(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpContextAccessor();

            services.ConfigureContextNpgsql(configuration);
            services.ConfigureRedis(configuration);
            services.ConfigureRabbit(configuration);
            services.AddMediatR(cfg =>
         cfg.RegisterServicesFromAssembly(typeof(CreatePointCommand).Assembly));

            services.AddScoped<IAppConfiguration, AppConfiguration>();
            services.AddScoped<IRepository<Country>, Repository<Country>>();
            services.AddScoped<IRepository<Province>, Repository<Province>>();
            services.AddScoped<IRepository<Locality>, Repository<Locality>>();
            services.AddScoped<IRepository<Contact>, Repository<Contact>>();
            services.AddScoped<IRepository<WorkingHours>, Repository<WorkingHours>>();
            services.AddScoped<ICompanyPointRepository, CompanyPointRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



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
