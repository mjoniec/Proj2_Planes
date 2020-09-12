using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mqtt;
using Mqtt.Interfaces;
using System.Threading.Tasks;

namespace PlaneService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //for some reason on debug it acts as in production, see proj/env vars #22
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<MqttConfig>(hostContext.Configuration.GetSection("Mqtt"));//same section for server and client

                    //adds client to reveive messages
                    services.AddSingleton<IMqttClientSubscriber, MqttClientSubscriber>();

                    //adds service to run plane logic 
                    services.AddHostedService<PlaneService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                 {
                     //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                 });

            await builder.RunConsoleAsync();
        }
    }
}
