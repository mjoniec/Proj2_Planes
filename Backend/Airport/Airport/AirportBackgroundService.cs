using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Airport
{
    class AirportBackgroundService : AirTrafficBackgroundService
    {
        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(new AirportService(configuration, hostEnvironment))
        {

        }
    }
}
