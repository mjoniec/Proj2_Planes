using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Utils;

namespace PlaneService.Domain
{
    public class PlaneLifetimeManager
    {
        private readonly ILogger<PlaneLifetimeManager> _logger;
        private readonly Plane _plane;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdatePlaneUrl;
        private readonly string TrafficInfoApiGetAirportUrl;
        private readonly string TrafficInfoApiGetAirportsUrl;

        public PlaneLifetimeManager(string name, string trafficInfoApiUpdatePlaneUrl, 
            string trafficInfoApiGetAirportUrl, string trafficInfoApiGetAirportsUrl)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<PlaneLifetimeManager>();
            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            TrafficInfoApiUpdatePlaneUrl = trafficInfoApiUpdatePlaneUrl;
            TrafficInfoApiGetAirportUrl = trafficInfoApiGetAirportUrl;
            TrafficInfoApiGetAirportsUrl = trafficInfoApiGetAirportsUrl;
        }

        public async Task Start()
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiGetAirportsUrl));
        }

        public async Task Loop()
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
        }

        private async Task SelectNewDestinationAirport()
        {
            _logger.LogInformation("SelectNewDestinationAirport");

            var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiGetAirportsUrl);

            _plane.SelectNewDestinationAirport(airports);
        }
    }
}
