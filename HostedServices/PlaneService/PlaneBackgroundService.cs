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
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string AirTrafficApiUpdatePlaneInfoUrl;
        private readonly string AirTrafficApiGetAirportsUrl;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", _hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));

            _plane = new Plane(name);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();

            _hostEnvironment = hostEnvironment;
            AirTrafficApiUpdatePlaneInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdatePlaneInfoUrl));
            AirTrafficApiGetAirportsUrl = configuration.GetValue<string>(nameof(AirTrafficApiGetAirportsUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(AirTrafficApiGetAirportsUrl));

            while (!stoppingToken.IsCancellationRequested)
            {
                //should plane management logic from these few lines go to a separate domain lifecycle manager, 
                //exported as a tick for a loop holder object? ?

                _plane.UpdatePlane();

                if (_plane.PlaneReachedItsDestination)
                {
                    var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(AirTrafficApiGetAirportsUrl);

                    _plane.SelectNewDestinationAirport(airports);
                }

                await _trafficInfoHttpClient.PostPlaneInfo(_plane.PlaneContract, AirTrafficApiUpdatePlaneInfoUrl);
                await Task.Delay(600, stoppingToken);
            }
        }
    }
}
