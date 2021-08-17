using AirTrafficInfoContracts;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulatedTrafficHostedService
{
    public class SimulatedTrafficBackgroundService : BackgroundService
    {
        //private readonly IHostEnvironment _hostEnvironment;
        private readonly string _trafficApiUpdateAirportUrl;
        private readonly string _trafficApiUpdatePlaneUrl;
        private readonly HttpClient _httpClient;
        private readonly AirTrafficInfoContract _airTrafficInfoContract;

        public SimulatedTrafficBackgroundService(IHostEnvironment hostEnvironment)
        {
            _trafficApiUpdateAirportUrl = hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/AirTrafficInfo/UpdateAirportInfo"
                : $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdateAirportInfo";//to be tested ...

            //_hostEnvironment = hostEnvironment;
            //TODO: refactor these urls after deployment is figured out
            _trafficApiUpdatePlaneUrl = hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/AirTrafficInfo/UpdatePlaneInfo"
                : $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdatePlaneInfo";//to be tested ...

            _airTrafficInfoContract = AirTrafficInfoContractDataFactory.Get();
            _httpClient = new HttpClient();

            SendAirportsToTrafficApi();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                MovePlanes();

                SendPlanesToTrafficApi();

                await Task.Delay(4000, stoppingToken);
            }
        }

        private void SendAirportsToTrafficApi()
        {
            _airTrafficInfoContract.Airports.ForEach(a => SendAirportToTrafficApi(a));
        }

        private void SendAirportToTrafficApi(AirportContract airport)
        {
            _httpClient.PostAsync(
                _trafficApiUpdateAirportUrl,
                new StringContent(JsonConvert.SerializeObject(airport),
                Encoding.UTF8, "application/json"));
        }

        private void SendPlanesToTrafficApi()
        {
            _airTrafficInfoContract.Planes.ForEach(p => SendPlaneUpdateToTrafficApi(p));
        }

        private void SendPlaneUpdateToTrafficApi(PlaneContract plane)
        {
            _httpClient.PostAsync(
                _trafficApiUpdatePlaneUrl,
                new StringContent(JsonConvert.SerializeObject(plane),
                Encoding.UTF8, "application/json"));
        }

        private void MovePlanes()
        {
            _airTrafficInfoContract.Planes.ForEach(p => MovePlane(p));
        }

        private void MovePlane(PlaneContract planeContract)
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
