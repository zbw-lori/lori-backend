using lori.backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lori.backend.Infrastructure;
public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString) =>
      services.AddDbContext<LoriDbContext>(options =>
          options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // will be created in web project root
}
