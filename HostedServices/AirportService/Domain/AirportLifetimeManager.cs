using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Utils;

namespace AirportService.Domain
{
    public class AirportLifetimeManager
    {
        private readonly ILogger<AirportLifetimeManager> _logger;
        private readonly Airport _airport;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;
        private readonly string TrafficInfoApiUpdateAirportUrl;

        public AirportLifetimeManager(
            string name, string color, string latitude, string longitude,
            string trafficInfoApiUpdateAirportUrl)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<AirportLifetimeManager>();
            _airport = new Airport(name, color, latitude, longitude);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            TrafficInfoApiUpdateAirportUrl = trafficInfoApiUpdateAirportUrl;
        }

        public async Task Loop()
        {
            _logger.LogInformation(_airport.AirportContract.Name + " Execute loop at " + DateTime.Now.ToString("G"));

            await _airport.UpdateAirport();
            await _trafficInfoHttpClient.PostAirportInfo(_airport.AirportContract, TrafficInfoApiUpdateAirportUrl);
        }
    }
}
