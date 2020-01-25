using JNogueira.Logger.Discord;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JNogueira.Bufunfa.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
