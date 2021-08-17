using AirTrafficInfoContracts;
using Microsoft.Extensions.Configuration;//TODO get rid of these http specific clients, export to shared kernel expose through interface
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// Comment for PR:
    /// - Domain object Plane should not have http client directly and handle connection logic - it should have airports data query or sth ...
    /// - Domain object Plane should not have stopping token, that logic belongs in host service or worker, plane should expose how to start update and stop it, business domain behavior
    /// - navigation (shared kernel?) class have move plane method, that is leaking language, it should have move point x by speed y on time t etc ...
    /// - navigation in plane or domain specific assumption (like accuracy when we consider destination as reached) should be in plane
    /// - plane and its navigation should be abstract to its host service, usable from simulated traffic and actual background worker 
    /// </summary>

    public class Plane
    {
        private readonly string AirTrafficApiUpdatePlaneInfoUrl;
        private readonly string AirTrafficApiGetAirportsUrl;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private PlaneContract _planeContract;

        public Plane(IConfiguration configuration, IHostEnvironment hostEnvironment)
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

        /// <summary>
        /// Setup Destination and Departure Airports for the Plane
        /// </summary>
        /// <returns></returns>
        public async Task StartPlane()
        {
            //any way not to duplicate this code with method below?
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

        public async Task UpdatePlane()
        {
            var currentTime = DateTime.Now;

            Navigation.MovePlane(ref _planeContract, currentTime);

            _planeContract.LastPositionUpdate = currentTime;

            if (HasPlaneReachedItsDestination())
            {
                await SelectNewDestinationAirport();
            }
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

        private bool HasPlaneReachedItsDestination()
        {
            return Navigation.HasPlaneReachedItsDestination(
                _planeContract.DestinationAirportLatitude,
                _planeContract.DestinationAirportLongitude,
                _planeContract.Latitude,
                _planeContract.Longitude);
        }

        /// <summary>
        /// we do not want the name on production to have anything other than pilot name 
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
