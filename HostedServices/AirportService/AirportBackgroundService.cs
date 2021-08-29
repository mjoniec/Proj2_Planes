using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly ILogger<AirportBackgroundService> _logger;
        private readonly AirportLifetimeManager _airportLifetimeManager;
        
        public AirportBackgroundService(
            ILogger<AirportBackgroundService> logger,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            _logger = logger;

            var name = HostServiceNameSelector.AssignName("Airport",
                hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");
            var trafficInfoApiUpdateAirportUrl = configuration.GetValue<string>("TrafficInfoApiUpdateAirportUrl");

            _airportLifetimeManager = new AirportLifetimeManager(
                name, color, latitude, longitude,
                trafficInfoApiUpdateAirportUrl);

            _logger.LogInformation("Created AirportBackgroundService for: " + name);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _airportLifetimeManager.Loop();
                await Task.Delay(4000, stoppingToken);
            }

            _logger.LogInformation("CancellationRequested");
        }
    }
}
