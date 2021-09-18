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
        private readonly string UpdatePlaneUrl;
        private readonly string AddPlaneUrl;
        private readonly string DeletePlaneUrl;
        private readonly string GetAirportUrl;
        private readonly string GetAirportsUrl;

        public PlaneLifetimeManager(string name, string updatePlaneUrl, string addPlaneUrl,
            string getAirportUrl, string getAirportsUrl, string deletePlaneurl)
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
            DeletePlaneUrl = deletePlaneurl;
            GetAirportUrl = getAirportUrl;
            GetAirportsUrl = getAirportsUrl;
        }

        public async Task<bool> Start()
        {
            _logger.LogInformation(_plane.PlaneContract.Name + " start plane at " + DateTime.Now.ToString("G"));
            _plane.StartPlane(await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(GetAirportsUrl));

            return await KeepTryingToAddPlaneUntilSuccessful();
        }

        public async Task Stop()
        {
            _logger.LogInformation("Stopping plane " + _plane.PlaneContract.Name);
            
            await _trafficInfoHttpClient.DeletePlane(DeletePlaneUrl, _plane.PlaneContract.Name);
        }

        public async Task Loop()
        {
            _logger.LogInformation(_plane.PlaneContract.Name + " update loop at " + DateTime.Now.ToString("G"));

            _plane.UpdatePlane();

            if (string.IsNullOrEmpty(_plane.PlaneContract.DestinationAirportName))
            {
                _logger.LogWarning("No airport destination selected for plane: " + _plane.PlaneContract.Name);

                await SelectNewDestinationAirport();

                return;
            }

            var airport = await _trafficInfoHttpClient.GetAirport(
                GetAirportUrl, _plane.PlaneContract.DestinationAirportName);

            if (airport == null)
            {
                _logger.LogError("No airport " + _plane.PlaneContract.DestinationAirportName + 
                    " found as next destination for plane: " + _plane.PlaneContract.Name);

                await SelectNewDestinationAirport();

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

            var response = await _trafficInfoHttpClient.UpdatePlane(_plane.PlaneContract, UpdatePlaneUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("update plane unsuccessful");
            }
        }

        private async Task SelectNewDestinationAirport()
        {
            _logger.LogInformation("Selecting New Destination Airport");

            var airports = await _trafficInfoHttpClient.GetCurrentlyAvailableAirports(GetAirportsUrl);

            _plane.SelectNewDestinationAirport(airports);
        }

        //#33
        //this method logic does not belong in this manager responsibility 
        //and needs to be extracted to some service - 
        //service name d indicate that we want to retry unsuccessful adding
        public async Task<bool> KeepTryingToAddPlaneUntilSuccessful()
        {
            while (true)
            {
                _logger.LogInformation("trying to add plane " + _plane.PlaneContract.Name);

                try
                {
                    var response = await _trafficInfoHttpClient.AddPlane(_plane.PlaneContract, AddPlaneUrl);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation("add plane successful");

                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("add plane unsuccessful - response not ok - retrying");

                        await Task.Delay(5000);
                    }
                }
                catch (TaskCanceledException e)
                {
                    _logger.LogWarning("add plane unsuccessful with expected TaskCanceledException " + e.Message + " - retrying");
                }
                catch (Exception e)
                {
                    _logger.LogWarning("add plane unsuccessful with unexpected Exception " + e.Message + " - stopping retrying");

                    return false;
                }

            }
        }
    }
}
