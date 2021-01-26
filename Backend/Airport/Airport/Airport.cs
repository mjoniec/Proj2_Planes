using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AirTrafficInfoServices;

namespace Airport
{
    public class Airport : AirTrafficBackgroundService
    {
        public Airport(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(new AirportService(configuration, hostEnvironment))
        {
            //works on premises ps launch - dotnet run --color=888111
            //var color = configuration.GetValue<string>("color");
            //Console.WriteLine(color + " test");
        }
    }
}
