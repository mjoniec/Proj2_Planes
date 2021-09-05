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
        private readonly string UpdateAirportUrl;
        private readonly string AddAirportUrl;

        public AirportLifetimeManager(
            string name, string color, string latitude, string longitude,
            string updateAirportUrl, string addAirportUrl)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger<AirportLifetimeManager>();
            _airport = new Airport(name, color, latitude, longitude);
            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            UpdateAirportUrl = updateAirportUrl;
            AddAirportUrl = addAirportUrl;
        }

        public async Task Start()
        {
            _logger.LogInformation(_airport.AirportContract.Name + " start airport at " + DateTime.Now.ToString("G"));

            await _trafficInfoHttpClient.KeepTryingAddAirportUntilSuccessful(_airport.AirportContract, AddAirportUrl);
        }

        public async Task Loop()
        {
            _logger.LogInformation(_airport.AirportContract.Name + " Execute loop at " + DateTime.Now.ToString("G"));

            await _airport.UpdateAirport();
            
            var response = await _trafficInfoHttpClient.UpdateAirport(_airport.AirportContract, UpdateAirportUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("update airport unsuccessful");
            }
        }
    }
}
