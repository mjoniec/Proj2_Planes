using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Plane
{
    public class PlaneBackgroundService : AirTrafficBackgroundService
    {
        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(new PlaneService(configuration, hostEnvironment))
        {
            
        }
    }
}
