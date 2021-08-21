using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace AirportService
{
    public class AirportBackgroundService : BackgroundService
    {
        private readonly Airport _airport;

        private readonly HttpClient _httpClient;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string AirTrafficApiUpdateAirportInfoUrl;

        public AirportBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            //required to install nuget: Microsoft.Extensions.Configuration.Binder
            var name = HostServiceNameSelector.AssignName("Plane", _hostEnvironment.EnvironmentName, configuration.GetValue<string>("name"));
            var color = configuration.GetValue<string>("color");
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            _airport = new Airport(name, color, latitude, longitude);
            _httpClient = new HttpClient();
            _hostEnvironment = hostEnvironment;
            AirTrafficApiUpdateAirportInfoUrl = configuration.GetValue<string>(nameof(AirTrafficApiUpdateAirportInfoUrl));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _airport.UpdateAirport();

                await _httpClient.PostAsync(
                    AirTrafficApiUpdateAirportInfoUrl, //TODO get rid of these http specific clients, export to shared kernel expose through interface
                    new StringContent(JsonConvert.SerializeObject(_airportContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }
    }
}
