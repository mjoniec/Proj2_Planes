using Domain;
using HttpUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PlaneService
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly Plane _plane;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdatePlaneUrl;
        private readonly string TrafficInfoApiUpdateGetAirportsUrl;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));

            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();

            TrafficInfoApiUpdatePlaneUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdatePlaneUrl));
            TrafficInfoApiUpdateGetAirportsUrl = configuration.GetValue<string>(nameof(TrafficInfoApiUpdateGetAirportsUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiUpdateGetAirportsUrl));

            while (!stoppingToken.IsCancellationRequested)
            {
                //should plane management logic from these few lines go to a separate domain lifecycle manager, 
                //exported as a tick for a loop holder object? ?

                _plane.UpdatePlane();

                if (_plane.PlaneReachedItsDestination)
                {
                    var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(TrafficInfoApiUpdateGetAirportsUrl);

                    _plane.SelectNewDestinationAirport(airports);
                }

                await _trafficInfoHttpClient.PostPlaneInfo(_plane.PlaneContract, TrafficInfoApiUpdatePlaneUrl);
                await Task.Delay(800, stoppingToken);
            }
        }
    }
}
