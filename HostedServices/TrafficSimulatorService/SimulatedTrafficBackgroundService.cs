using AirportService.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PlaneService.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficSimulatorService
{
    public class SimulatedTrafficBackgroundService : BackgroundService
    {
        private readonly List<PlaneLifetimeManager> _planesManagers;
        private readonly List<AirportLifetimeManager> _airportsManagers;

        public SimulatedTrafficBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var updateAirportUrl = configuration.GetValue<string>("UpdateAirportUrl");
            var addAirportUrl = configuration.GetValue<string>("AddAirportUrl");
            var updatePlaneUrl = configuration.GetValue<string>("UpdatePlaneUrl");
            var addPlaneUrl = configuration.GetValue<string>("AddPlaneUrl");
            var getAirportUrl = configuration.GetValue<string>("GetAirportUrl");
            var getAirportsUrl = configuration.GetValue<string>("GetAirportsUrl");

            _airportsManagers = DomainObjectsDataFactory.GetAirportLifetimeManagers(updateAirportUrl, addAirportUrl).Result;

            Task.Delay(2000);//concurrent requests from planes may ask for data not yet setup

            _planesManagers = DomainObjectsDataFactory.GetPlaneLifetimeManagers(updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _planesManagers.ForEach(async p => await p.Start());

            await Task.Delay(2000, stoppingToken);//concurrent requests from planes may ask for data not yet setup

            _airportsManagers.ForEach(async p => await p.Start());

            while (!stoppingToken.IsCancellationRequested)
            {
                _airportsManagers.ForEach(async a => await a.Loop());
                _planesManagers.ForEach(async p => await p.Loop());

                await Task.Delay(4000, stoppingToken);
            }
        }
    }
}
