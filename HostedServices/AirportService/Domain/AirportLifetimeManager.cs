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
        private readonly string DeleteAirportUrl;

        public AirportLifetimeManager(
            string name, string color, string latitude, string longitude,
            string updateAirportUrl, string addAirportUrl, string deleteAirportUrl)
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
            DeleteAirportUrl = deleteAirportUrl;
        }

        public async Task<bool> Start()
        {
            _logger.LogInformation(_airport.AirportContract.Name + " start airport at " + DateTime.Now.ToString("G"));

            return await KeepTryingToAddAirportUntilSuccessful();
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

        public async Task Remove()
        {
            _logger.LogInformation("Removing airport " + _airport.AirportContract.Name);

            await _trafficInfoHttpClient.DeleteAirport(DeleteAirportUrl, _airport.AirportContract.Name);
        }

        //#33
        //this method logic does not belong in this manager responsibility 
        //and needs to be extracted to some service - 
        //service name d indicate that we want to retry unsuccessful adding
        private async Task<bool> KeepTryingToAddAirportUntilSuccessful()
        {
            while (true)
            {
                _logger.LogInformation("trying to add airport " + _airport.AirportContract.Name);
                
                try
                {
                    var response = await _trafficInfoHttpClient.AddAirport(_airport.AirportContract, AddAirportUrl);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation("add airport successful");

                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("add airport unsuccessful - response not ok - retrying");

                        await Task.Delay(5000);
                    }
                }
                catch (TaskCanceledException e)
                {
                    _logger.LogWarning("add airport unsuccessful with expected TaskCanceledException " + e.Message + " - retrying");
                }
                catch (Exception e)
                {
                    _logger.LogWarning("add airport unsuccessful with unexpected Exception " + e.Message + " - stopping retrying");

                    return false;
                }
            }
        }
    }
}
