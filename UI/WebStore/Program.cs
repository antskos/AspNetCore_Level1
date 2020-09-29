using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseStartup<Startup>();
                    host.ConfigureLogging((host, log) => {          // возможность конфигурировать логирование в коде
                        // log.ClearProviders();
                        // log.AddProvider();
                        // log.AddConsole(opt => opt.IncludeScopes = true);
                        // log.AddFilter(level => level >= LogLevel.Information);
                        // log.AddFilter("Microsoft", level >= LogLevel.Warning);   // для конкретной категории фильтр
                    });
                });
    }
}
