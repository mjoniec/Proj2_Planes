using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlaneService.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PlaneService
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly ILogger<PlaneBackgroundService> _logger;
        private readonly Plane _plane;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdatePlaneUrl;
        private readonly string TrafficInfoApiGetAirportUrl;
        private readonly string TrafficInfoApiGetAirportsUrl;

        public PlaneBackgroundService(
            ILogger<PlaneBackgroundService> logger,
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));

            _logger = logger;
            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            
            TrafficInfoApiUpdatePlaneUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdatePlaneUrl));
            TrafficInfoApiGetAirportUrl = configuration.GetValue<string>(nameof(TrafficInfoApiGetAirportUrl));
            TrafficInfoApiGetAirportsUrl = configuration.GetValue<string>(nameof(TrafficInfoApiGetAirportsUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiGetAirportsUrl));

            //plane management logic should go to a separate domain lifecycle manager, exported as a tick for a loop holder object
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(_plane.PlaneContract.Name + " update loop at " + DateTime.Now.ToString("G"));

                _plane.UpdatePlane();

                var airport = await _trafficInfoHttpClient.GetAirport(
                    TrafficInfoApiGetAirportUrl, _plane.PlaneContract.DestinationAirportName);

                if (!airport.IsGoodWeather)
                {
                    _logger.LogInformation("Plane's Destination Bad Weather");

                    await SelectNewDestinationAirport();
                }

                if (_plane.PlaneReachedItsDestination)
                {
                    _logger.LogInformation("Plane Reached Its Destination");

                    await SelectNewDestinationAirport();
                }

                await _trafficInfoHttpClient.PostPlaneInfo(_plane.PlaneContract, TrafficInfoApiUpdatePlaneUrl);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task SelectNewDestinationAirport()
        {
            _logger.LogInformation("SelectNewDestinationAirport");

            var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiGetAirportsUrl);

            _plane.SelectNewDestinationAirport(airports);
        }
    }
}
