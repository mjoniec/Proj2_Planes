using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using AirTrafficInfoServices;

namespace Airport
{
    public class Airport : AirTrafficBackgroundService
    {
        private readonly IConfiguration _configuration;
        
        public Airport(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(new AirportService(hostEnvironment))
        {
            _configuration = configuration;
        }
    }
}
