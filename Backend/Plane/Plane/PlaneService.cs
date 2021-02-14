using AirTrafficInfoContracts;
using AirTrafficInfoServices;
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
    public class PlaneService : IAirTrafficService
    {
        private readonly string AirTrafficApiUpdatePlaneInfoUrl;
        private readonly string AirTrafficApiGetAirportsUrl;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private PlaneContract _planeContract;
        
        public PlaneService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = configuration.GetValue<string>("name");

            AirTrafficApiUpdatePlaneInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdatePlaneInfoUrl));
            AirTrafficApiGetAirportsUrl = configuration.GetValue<string>(nameof(AirTrafficApiGetAirportsUrl));

            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _planeContract = new PlaneContract
            {
                Name = AssignName(name),
                SpeedInMetersPerSecond = 1000000
            };
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetupDestinationAndDepartureAirportsForNewPlane();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdatePlane();

                await _httpClient.PostAsync(
                    AirTrafficApiUpdatePlaneInfoUrl,
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
            var response = await _httpClient.GetAsync(AirTrafficApiGetAirportsUrl);
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

        /// <summary>
        /// we do not want the name on production to have anything other than city name or pilots name 
        /// we also want to see more info easily on non production environments
        /// </summary>
        /// <param name="name"></param>
        private string AssignName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (_hostEnvironment.EnvironmentName == "Development")//manual on premises launch from visual studio
                {
                    name = "Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
                else if (_hostEnvironment.EnvironmentName == "Docker")
                {
                    name = "Error - PlaneNameShouldHaveBeenGivenFor_" + _hostEnvironment.EnvironmentName + "_Environment_" + new Random().Next(1001, 9999).ToString();
                }
                else
                {
                    name = "Warning - Unpredicted Environment - Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }
            else
            {
                if (_hostEnvironment.EnvironmentName == "Development")//on premises launch from ps script
                {
                    name += "_" + _hostEnvironment.EnvironmentName;
                }
                else if (_hostEnvironment.EnvironmentName == "Docker")
                {
                    //production name - expected to be displayed as given from docker compose
                }
                else
                {
                    name += "Warning - Unpredicted Environment - Plane_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString();
                }
            }

            return name;
        }
    }
}
