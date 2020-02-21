using JNogueira.Logger.Discord;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JNogueira.Bufunfa.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((hostingContext, logging) =>
                {
                    if (!hostingContext.HostingEnvironment.IsProduction())
                    {
                        logging.AddFilter<DiscordLoggerProvider>("Microsoft", LogLevel.Error);
                        logging.AddConsole();
                    }
                    else
                    {
                        logging.AddFilter<DiscordLoggerProvider>("Microsoft", LogLevel.Warning);
                    }

                    logging.AddFilter<DiscordLoggerProvider>("JNogueira.Bufunfa.Web", LogLevel.Warning);
                    logging.AddFilter<DiscordLoggerProvider>("Microsoft.AspNetCore.Antiforgery", LogLevel.None);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
