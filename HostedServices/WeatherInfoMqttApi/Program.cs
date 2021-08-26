using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttUtils;

namespace WeatherInfoMqttApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<MqttConfig>(hostContext.Configuration.GetSection("MqttConfig"));
                    services.AddHostedService<MqttServerHostedService>();
                });
    }
}
