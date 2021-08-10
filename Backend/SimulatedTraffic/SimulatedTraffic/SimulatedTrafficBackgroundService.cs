using AirTrafficInfoContracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SimulatedTraffic
{
    public class SimulatedTrafficBackgroundService : BackgroundService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly AirTrafficInfoContract _airTrafficInfoContract;

        public SimulatedTrafficBackgroundService(IHostEnvironment hostEnvironment)
        {
            _airTrafficInfoContract = AirTrafficInfoContractDataFactory.Get();
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: refactor these urls after deployment is figured out
            var url = _hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/AirTrafficInfo"
                : $"http://mockairtraffic.azurewebsites.net/api/mockAirTrafficInfo";

            while (!stoppingToken.IsCancellationRequested)
            {
                UpdateTraffic();

                await _httpClient.PostAsync(url, null);

                await Task.Delay(1100, stoppingToken);
            }
        }

        private void UpdateTraffic()
        {
            _airTrafficInfoContract.Planes.ForEach(p => UpdatePlane(p));
        }

        private void UpdatePlane(PlaneContract planeContract)
        {
            //this is supposed to be calculated in mock system worker - export and this is to update only
            var currentTime = DateTime.Now;

            Navigation.MovePlane(planeContract, currentTime);

            planeContract.LastPositionUpdate = currentTime;

            if (HasPlaneReachedItsDestination(planeContract))
            {
                SelectNewDestinationAirport(planeContract);
            }
        }

        private void SelectNewDestinationAirport(PlaneContract planeContract)
        {
            var airports = _airTrafficInfoContract.Airports;
            var randomDestinationAirport = SelectRandomAirportWithException(airports, planeContract.DestinationAirportName);

            planeContract.SetNewDestinationAndDepartureAirports(randomDestinationAirport);
            planeContract.DepartureTime = DateTime.Now;
        }

        private AirportContract SelectRandomAirportWithException(List<AirportContract> airports, string exceptThisAirportName)
        {
            var airportsWithoutException = new List<AirportContract>(airports);

            airportsWithoutException.RemoveAll(a => a.Name == exceptThisAirportName);

            return airportsWithoutException[new Random().Next(0, airportsWithoutException.Count)];
        }

        private bool HasPlaneReachedItsDestination(PlaneContract planeContract)
        {
            return (Math.Abs(planeContract.DestinationAirportLatitude - planeContract.Latitude) <= 1 &&
                Math.Abs(planeContract.DestinationAirportLongitude - planeContract.Longitude) <= 1);
        }
    }
}
