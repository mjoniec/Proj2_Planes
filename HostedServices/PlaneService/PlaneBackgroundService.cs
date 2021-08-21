using Contracts;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace PlaneService
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly Plane _plane;

        private readonly HttpClient _httpClient;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string AirTrafficApiUpdatePlaneInfoUrl;
        private readonly string AirTrafficApiGetAirportsUrl;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", _hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));

            _plane = new Plane(name);
            _httpClient = new HttpClient();
            _hostEnvironment = hostEnvironment;
            AirTrafficApiUpdatePlaneInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdatePlaneInfoUrl));
            AirTrafficApiGetAirportsUrl = configuration.GetValue<string>(nameof(AirTrafficApiGetAirportsUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _plane.StartPlane(await GetCurrentlyAvailableAirports());

            while (!stoppingToken.IsCancellationRequested)
            {
                _plane.UpdatePlane();//should plane management logic go to a separate domain lifecycle manager ?

                if (_plane.PlaneReachedItsDestination)
                {
                    var airports = await GetCurrentlyAvailableAirports();

                    _plane.SelectNewDestinationAirport(airports);
                }

                await PostPlaneInfo(_plane.PlaneContract);
                await Task.Delay(600, stoppingToken);
            }
        }

        private async Task PostPlaneInfo(PlaneContract planeContract)
        {
            await _httpClient.PostAsync(
                AirTrafficApiUpdatePlaneInfoUrl,
                new StringContent(JsonConvert.SerializeObject(planeContract),
                Encoding.UTF8, "application/json"));
        }

        private async Task<List<AirportContract>> GetCurrentlyAvailableAirports()
        {
            //TODO get rid of these http specific clients, export to shared kernel expose through interface, inject some service here
            var response = await _httpClient.GetAsync(AirTrafficApiGetAirportsUrl); 
            var json = await response.Content.ReadAsStringAsync();
            var airports = JsonConvert.DeserializeObject<List<AirportContract>>(json);

            return airports;
        }
    }
}
