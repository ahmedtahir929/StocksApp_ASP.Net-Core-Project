using Microsoft.EntityFrameworkCore;
using StocksApp_xUnit.Options;

namespace StocksApp_xUnit.StartupExtensions
{
  public static class ConfigureServicesExtension
  {
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
      services.AddHttpLogging();
      services.AddHttpClient();
      services.AddControllersWithViews();
      services.AddLogging();

      if (!webHostEnvironment.IsEnvironment("Test"))
      {
        services.AddDbContext<Entities.StockMarketDbContext>(options =>
        {
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
      }

      services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
      services.Configure<TradingApiOptions>(configuration.GetSection("TradingApiOptions"));

      return services;
    }
  }
}
