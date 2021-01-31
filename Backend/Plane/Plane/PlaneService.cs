using AirTrafficInfoContracts;
using AirTrafficInfoServices;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public class PlaneService : IAirTrafficService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private PlaneContract _planeContract;

        public PlaneService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _planeContract = new PlaneContract
            {
                Name = "Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString(),
                SpeedInMetersPerSecond = 3000000
            };
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: move to app settings 
            var url = _hostEnvironment.EnvironmentName == "Docker"
                ? $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdatePlaneInfo"
                : $"https://localhost:44389/api/airtrafficinfo/UpdatePlaneInfo";

            await SetupDestinationAndDepartureAirportsForNewPlane();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdatePlane();

                await _httpClient.PostAsync(
                    url,
                    new StringContent(JsonConvert.SerializeObject(_planeContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(600, stoppingToken);
            }
        }

        private async Task SetupDestinationAndDepartureAirportsForNewPlane()
        {
            var airports = await GetCurrentlyAvailableAirports();

            if (!AreEnoughAirportsToSelectNewDestination(airports))
            {
                EmptyDestinationAndDepartureAirports();

                return;
            }

            var randomDepartureAirport = SelectRandomAirport(airports);
            var randomDestinationAirport = SelectRandomAirportExceptTheOneProvided(airports, randomDepartureAirport.Name);

            _planeContract.SetDepartureAirportData(randomDepartureAirport);
            _planeContract.SetDestinationAirportData(randomDestinationAirport);
            _planeContract.DepartureTime = DateTime.Now;
            _planeContract.LastPositionUpdate = DateTime.Now;
        }

        private async Task SelectNewDestinationAirport()
        {
            var airports = await GetCurrentlyAvailableAirports();

            if (!AreEnoughAirportsToSelectNewDestination(airports))
            {
                EmptyDestinationAndDepartureAirports();

                return;
            }

            var randomDestinationAirport = SelectRandomAirportExceptTheOneProvided(airports, _planeContract.DestinationAirportName);

            _planeContract.SetNewDestinationAndDepartureAirports(randomDestinationAirport);
            _planeContract.DepartureTime = DateTime.Now;            
        }

        private async Task<List<AirportContract>> GetCurrentlyAvailableAirports()
        {
            var url = _hostEnvironment.EnvironmentName == "Docker"
                ? $"http://airtrafficinfo_1:80/api/airtrafficinfo/GetAirports"
                : $"https://localhost:44389/api/airtrafficinfo/GetAirports";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var airports = JsonConvert.DeserializeObject<List<AirportContract>>(json);

            return airports;
        }

        private void EmptyDestinationAndDepartureAirports()
        {
            _planeContract.DestinationAirportName = string.Empty;
            _planeContract.DepartureAirportName = string.Empty;
            _planeContract.Color = "#000000";
        }

        private bool AreEnoughAirportsToSelectNewDestination(List<AirportContract> airports)
        {
            return airports.Count >= 2;
        }

        private AirportContract SelectRandomAirport(List<AirportContract> airports)
        {
            return airports[new Random().Next(0, airports.Count)];
        }

        private AirportContract SelectRandomAirportExceptTheOneProvided(List<AirportContract> airports, string exceptThisAirportName)
        {
            var airportsWithoutException = new List<AirportContract>(airports);

            airportsWithoutException.RemoveAll(a => a.Name == exceptThisAirportName);

            return airportsWithoutException[new Random().Next(0, airportsWithoutException.Count)];
        }

        private async Task UpdatePlane()
        {
            var currentTime = DateTime.Now;

            Navigation.MovePlane(ref _planeContract, currentTime);

            _planeContract.LastPositionUpdate = currentTime;

            if (HasPlaneReachedItsDestination())
            {
                await SelectNewDestinationAirport();
            }
        }

        private bool HasPlaneReachedItsDestination()
        {
            return (Math.Abs(_planeContract.DestinationAirportLatitude - _planeContract.Latitude) <= 1 &&
                Math.Abs(_planeContract.DestinationAirportLongitude - _planeContract.Longitude) <= 1);
        }
    }
}
