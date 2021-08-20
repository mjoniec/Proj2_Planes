using Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private PlaneContract _planeContract;

        public Plane(string name)
        {
            _planeContract = new PlaneContract
            {
                Name = name,
                SpeedInMetersPerSecond = 1000000 //business constant - speed is as such for updates to be notable
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
            var response = await _httpClient.GetAsync(AirTrafficApiGetAirportsUrl); //TODO get rid of these http specific clients, export to shared kernel expose through interface, inject some service here
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
    }
}
