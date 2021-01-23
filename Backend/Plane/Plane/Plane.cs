using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Plane
{
    public class Plane : AirTrafficBackgroundService
    {
        private readonly IConfiguration _configuration;
        
        public Plane(IConfiguration configuration, IHostEnvironment hostEnvironment) 
            : base(new PlaneService(hostEnvironment))
        {
            _configuration = configuration;
        }
    }
}
