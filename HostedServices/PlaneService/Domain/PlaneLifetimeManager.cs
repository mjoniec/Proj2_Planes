﻿using Microsoft.Extensions.Logging;
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
        private readonly string UpdatePlaneUrl;
        private readonly string AddPlaneUrl;
        private readonly string GetAirportUrl;
        private readonly string GetAirportsUrl;

        public PlaneLifetimeManager(string name, string updatePlaneUrl, string addPlaneUrl,
            string getAirportUrl, string getAirportsUrl)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<PlaneLifetimeManager>();
            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            UpdatePlaneUrl = updatePlaneUrl;
            AddPlaneUrl = addPlaneUrl;
            GetAirportUrl = getAirportUrl;
            GetAirportsUrl = getAirportsUrl;
        }

        public async Task Start()
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(GetAirportsUrl));
            await _trafficInfoHttpClient.AddPlane(_plane.PlaneContract, AddPlaneUrl);
        }

        public async Task Loop()
        {
            _logger.LogInformation(_plane.PlaneContract.Name + " update loop at " + DateTime.Now.ToString("G"));

            _plane.UpdatePlane();

            var airport = await _trafficInfoHttpClient.GetAirport(
                GetAirportUrl, _plane.PlaneContract.DestinationAirportName);

            if (airport == null)
            {
                _logger.LogError("No airport destination for plane: " + _plane.PlaneContract.Name);

                return;
            }

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

            await _trafficInfoHttpClient.UpdatePlane(_plane.PlaneContract, UpdatePlaneUrl);
        }

        private async Task SelectNewDestinationAirport()
        {
            _logger.LogInformation("SelectNewDestinationAirport");

            var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(GetAirportsUrl);

            _plane.SelectNewDestinationAirport(airports);
        }
    }
}