using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mqtt;

namespace AirportService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //for some reason on debug it acts as in production, see proj/env vars #22
                    //no longer in web api project type - add cs proj anyway or use some other method?
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
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
                    
                    //adds mqtt server functionality
                    services.AddSingleton<IHostedService, MqttService>();
                    
                    //adds client to publish messages
                    services.AddSingleton<IMqttClient>(x =>
                    {
                        return new MqttClient(new MqttConfig
                        {
                            Ip = hostContext.Configuration["Mqtt:Ip"],
                            Port = int.Parse(hostContext.Configuration["Mqtt:Port"]),
                            Topic = hostContext.Configuration["Mqtt:Topic"]
                        });
                    });
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });

            await builder.RunConsoleAsync();
        }
    }
}
