
using Domain.Interfaces;
using Domain.Models;
using Email.API.Extensions;
using Infrastructure.Services;
namespace Email.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddEnvironmentVariables();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.ConfigureRabbit(builder.Configuration);
            builder.Services.AddSingleton<IEmailService, EmailService>();
            var app = builder.Build();
            app.Run();
        }
    }
}
