using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly IAirportService _service;

        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _service = new AirportService(configuration, hostEnvironment);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.ExecuteAsync(stoppingToken);
        }
    }
}
