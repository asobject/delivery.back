
using Application.Features.Chat.Commands.GenerateAi;
using BuildingBlocks.Exceptions;
using Domain.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace Chat.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.Configure<OllamaSettings>(
    builder.Configuration.GetSection(OllamaSettings.SectionName));

            builder.Services.AddHttpClient("Ollama", (sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<OllamaSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            });
            builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GenerateAiCommand).Assembly));
            builder.Services.AddTransient<GlobalExceptionMiddleware>();

            builder.Services.AddScoped<IAiResponseGenerator, OllamaResponseGenerator>();
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
