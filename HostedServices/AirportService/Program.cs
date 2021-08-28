using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttUtils;

namespace AirportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<MqttConfig>(hostContext.Configuration.GetSection("MqttConfig"));
                    services.AddSingleton<Airport>();
                    services.AddSingleton<MqttClientPublisher>();
                    services.AddHostedService<AirportBackgroundService>();
                });
    }
}
