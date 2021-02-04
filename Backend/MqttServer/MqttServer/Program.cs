using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttCommon;

namespace MqttServer
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
                    services.Configure<MqttConfig>(hostContext.Configuration.GetSection("Mqtt"));//same section for server and client
                    services.AddHostedService<MqttServer>();
                });
    }
}
