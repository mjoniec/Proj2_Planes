using AirTrafficInfoContracts;
using Microsoft.Extensions.Configuration;
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
    public class Plane : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;//needed for? just read settings
        private readonly HttpClient _httpClient;
        private readonly PlaneContract _planeContract;

        public Plane(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _planeContract = new PlaneContract
            {
                Name = "Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString()
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: move to app settings 
            var url = _hostEnvironment.EnvironmentName == "Docker"
                ? $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdatePlaneInfo"
                : $"https://localhost:44389/api/airtrafficinfo/UpdatePlaneInfo";

            _planeContract.Longitude = new Random().Next(1, 100);
            _planeContract.Latitude = new Random().Next(1, 100);
            _planeContract.SpeedInMetersPerSecond = 10000;

            await SetupDestinationAirportForNewPlane();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdatePlane();

                await _httpClient.PostAsync(
                    url,
                    new StringContent(JsonConvert.SerializeObject(_planeContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }

        private async Task SetupDestinationAirportForNewPlane()
        {
            var retryCount = 0;

            while (retryCount < 5)
            {
                await SelectNewDestinationAirport();

                if (_planeContract.DestinationAirport != null) break;

                retryCount++;
                Thread.Sleep(2000);
            }
        }

        private async Task SelectNewDestinationAirport()
        {
            var url = _hostEnvironment.EnvironmentName == "Docker"
                ? $"http://airtrafficinfo_1:80/api/airtrafficinfo/GetAirports"
                : $"https://localhost:44389/api/airtrafficinfo/GetAirports";

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var airports = JsonConvert.DeserializeObject<List<AirportContract>>(json);

            if(airports.Count <= 1)
            {
                return;
            }

            var random = new Random();

            while (true)
            {
                var nextAirport = airports[random.Next(0, airports.Count)];

                if (
                    _planeContract.DestinationAirport == null || //no destination so we can assign whatever
                    _planeContract.DestinationAirport.Name != nextAirport.Name)
                {
                    _planeContract.DepartureAirport = _planeContract.DestinationAirport;
                    _planeContract.DestinationAirport = nextAirport;

                    break;
                }
            }
        }

        private async Task UpdatePlane()
        {
            if(_planeContract.DestinationAirport.Latitude > _planeContract.Latitude)
            {
                _planeContract.Latitude++;
            }
            else if (_planeContract.DestinationAirport.Latitude < _planeContract.Latitude)
            {
                _planeContract.Latitude--;
            }

            if (_planeContract.DestinationAirport.Longitude > _planeContract.Longitude)
            {
                _planeContract.Longitude++;
            }
            else if (_planeContract.DestinationAirport.Longitude < _planeContract.Longitude)
            {
                _planeContract.Longitude--;
            }

            if (_planeContract.DestinationAirport.Latitude == _planeContract.Latitude &&
                _planeContract.DestinationAirport.Longitude == _planeContract.Longitude)
            {
                await SelectNewDestinationAirport();
            }
        }
    }
}
