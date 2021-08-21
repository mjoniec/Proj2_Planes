using Domain;
using HttpUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly Airport _airport;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string _airTrafficApiUpdateAirportInfoUrl;

        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Airport", _hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            _airport = new Airport(name, color, latitude, longitude);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            _hostEnvironment = hostEnvironment;
            _airTrafficApiUpdateAirportInfoUrl = configuration.GetValue<string>(nameof(_airTrafficApiUpdateAirportInfoUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _airport.UpdateAirport();
                await _trafficInfoHttpClient.PostAirportInfo(_airport.AirportContract, _airTrafficApiUpdateAirportInfoUrl);
                await Task.Delay(5100, stoppingToken);
            }
        }
    }
}
