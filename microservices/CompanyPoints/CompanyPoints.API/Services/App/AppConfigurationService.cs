using BuildingBlocks.Interfaces.Services;

namespace CompanyPoints.API.Services.App
{
    public class AppConfiguration(IConfiguration configuration) : IAppConfiguration
    {
        public string GetValue(string key)
        {
            return configuration[key]!;
        }
    }
}
