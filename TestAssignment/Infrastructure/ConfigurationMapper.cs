using Microsoft.Extensions.Configuration;

namespace TestAssignment.Infrastructure;

internal static class ConfigurationMapper
{
    internal static void MapTestConfig(IConfiguration config)
    {
        TestConfiguration.ApplicationConfig = config.GetSection("ApplicationConfig").Get<ApplicationConfig>();
        TestConfiguration.ApplicationConfig.Password = config.GetSection("ApplicationConfig")["Password"];
        TestConfiguration.ApplicationConfig.Email = config.GetSection("ApplicationConfig")["Email"];
    }
}