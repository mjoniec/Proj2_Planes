using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly ILogger<AirportBackgroundService> _logger;
        private readonly Airport _airport;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdateAirportUrl;

        public AirportBackgroundService(
            ILogger<AirportBackgroundService> logger,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            var name = HostServiceNameSelector.AssignName("Airport",
                hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            _airport = new Airport(name, color, latitude, longitude);

            _logger = logger;
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            TrafficInfoApiUpdateAirportUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdateAirportUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //airport management logic should go to a separate domain lifecycle manager, exported as a tick for a loop holder object
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(_airport.AirportContract.Name + " Execute loop at " + DateTime.Now.ToString("G"));

                await _airport.UpdateAirport();
                await _trafficInfoHttpClient.PostAirportInfo(_airport.AirportContract, TrafficInfoApiUpdateAirportUrl);
                await Task.Delay(4000, stoppingToken);
            }
        }
    }
}
