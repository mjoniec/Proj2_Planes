using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly Airport _airport;

        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _airport = new Airport(configuration, hostEnvironment);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _airport.ExecuteAsync(stoppingToken);
        }
    }
}
