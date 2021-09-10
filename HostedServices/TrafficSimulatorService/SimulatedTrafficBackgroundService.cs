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

        public SimulatedTrafficBackgroundService(IConfiguration configuration)
        {
            var updateAirportUrl = configuration.GetValue<string>("UpdateAirportUrl");
            var addAirportUrl = configuration.GetValue<string>("AddAirportUrl");
            var updatePlaneUrl = configuration.GetValue<string>("UpdatePlaneUrl");
            var addPlaneUrl = configuration.GetValue<string>("AddPlaneUrl");
            var getAirportUrl = configuration.GetValue<string>("GetAirportUrl");
            var getAirportsUrl = configuration.GetValue<string>("GetAirportsUrl");
            var deleteAirportUrl = configuration.GetValue<string>("DeleteAirportUrl");
            var deletePlaneUrl = configuration.GetValue<string>("DeletePlaneUrl");

            _airportsManagers = DomainObjectsDataFactory.GetAirportLifetimeManagers(updateAirportUrl, addAirportUrl, deleteAirportUrl);
            _planesManagers = DomainObjectsDataFactory.GetPlaneLifetimeManagers(updatePlaneUrl, addPlaneUrl, getAirportUrl, getAirportsUrl, deletePlaneUrl);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //#42
            //cannot use lambda and ForEach for all will get triggered at the same time
            //we want sequential init every second
            //this behavior causes some planes to fail on added and then continually fail on update loop - 
            //this is interesting fail async behavior that system should be bullet proof of
            //_airportsManagers.ForEach(async p =>
            //{
            //    await p.Start();
            //    await Task.Delay(1000);
            //});

            //_planesManagers.ForEach(async p =>
            //{
            //    await p.Start();
            //    await Task.Delay(1000);
            //});

            //init
            foreach (var a in _airportsManagers)
            {
                await a.Start();
                await Task.Delay(1000);
            }

            await Task.Delay(3000);

            foreach(var p in _planesManagers)
            {
                await p.Start();
                await Task.Delay(2000);
            }

            //keep on updating
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var a in _airportsManagers)
                {
                    await a.Loop();
                    await Task.Delay(300);
                }

                foreach (var p in _planesManagers)
                {
                    await p.Loop();
                    await Task.Delay(200);
                }
            }
        }
    }
}
