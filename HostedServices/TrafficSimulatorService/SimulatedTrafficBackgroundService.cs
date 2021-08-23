using Domain;
using HttpUtils;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficSimulatorService
{
    public class SimulatedTrafficBackgroundService : BackgroundService
    {
        private readonly string _trafficApiUpdateAirportUrl;
        private readonly string _trafficApiUpdatePlaneUrl;
        private readonly List<Plane> _planes;
        private readonly List<Airport> _airports;
        private readonly TrafficInfoHttpClient _trafficInfoHttpClient;

        public SimulatedTrafficBackgroundService(IHostEnvironment hostEnvironment)
        {
            _trafficApiUpdateAirportUrl = hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/AirTrafficInfo/UpdateAirportInfo"
                : $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdateAirportInfo";//to be tested ...

            //TODO: refactor these urls after deployment is figured out
            _trafficApiUpdatePlaneUrl = hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/AirTrafficInfo/UpdatePlaneInfo"
                : $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdatePlaneInfo";//to be tested ...

            _trafficInfoHttpClient = new TrafficInfoHttpClient();
            _airports = DomainObjectsDataFactory.GetAirports();
            _planes = DomainObjectsDataFactory.GetPlanes();

            //start all planes, it sets up departure and destination airports, else positions will show as 0
            var airportsContracts = _airports.Select(a => a.AirportContract).ToList();
            _planes.ForEach(p => p.StartPlane(airportsContracts));
            
            SendAirportsToTrafficApi();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UpdatePlanes();
                SendPlanesToTrafficApi();
                //for now airports do not change weather so no need for status update

                await Task.Delay(4000, stoppingToken);
            }
        }

        private void UpdatePlanes()
        {
            _planes.ForEach(p => p.UpdatePlane());
        }

        private void SendAirportsToTrafficApi()
        {
            _airports.ForEach(async a => await _trafficInfoHttpClient.PostAirportInfo(a.AirportContract, _trafficApiUpdateAirportUrl));
        }

        private void SendPlanesToTrafficApi()
        {
            _planes.ForEach(async p => await _trafficInfoHttpClient.PostPlaneInfo(p.PlaneContract, _trafficApiUpdatePlaneUrl));
        }
    }
}
