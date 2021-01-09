using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Plane
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
                    //var s = args[0]; //TODO: get number of container and paste to plane name
                    // on if condition if docker environment

                    services.AddHostedService<Plane>();
                });
    }
}
