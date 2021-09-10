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
            var updateAirportUrl = configuration.GetValue<string>("UpdateAirportUrl");
            var addAirportUrl = configuration.GetValue<string>("AddAirportUrl");
            var deleteAirportUrl = configuration.GetValue<string>("DeleteAirportUrl");

            _airportLifetimeManager = new AirportLifetimeManager(
                name, color, latitude, longitude,
                updateAirportUrl, addAirportUrl, deleteAirportUrl);

            _logger.LogInformation("Created AirportBackgroundService for: " + name);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting airport");

            await _airportLifetimeManager.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await _airportLifetimeManager.Loop();
                await Task.Delay(4000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CancellationRequested");

            await _airportLifetimeManager.Remove();
        }
    }
}
