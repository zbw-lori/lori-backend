using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Data;
using lori.backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lori.backend.Infrastructure;
public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<LoriDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // will be created in web project root
  }

  public static void AddMqttContext(this IServiceCollection services, string connectionString)
  {
    var values = GetKeyValuePairs(connectionString);
    services.AddSingleton<IMqttService, MqttService>(provider => new MqttService(values["clientId"], values["server"], int.Parse(values["port"])));
    services.AddSingleton<IMqttRegistrationService, MqttRegistrationService>();
    services.AddSingleton<IMqttLiveDataService, MqttLiveDataService>();
  }

  private static Dictionary<string, string> GetKeyValuePairs(string connectionString)
  {
    return connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new char[] { '=' }, 2))
            .ToDictionary(kvp => kvp[0].Trim(),
                          kvp => kvp[1].Trim(),
                          StringComparer.InvariantCultureIgnoreCase);
  }
}
