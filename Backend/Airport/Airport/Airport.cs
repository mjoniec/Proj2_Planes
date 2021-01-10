using AirTrafficInfoContracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public class Airport : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;//needed for? just read settings
        private readonly HttpClient _httpClient;
        private readonly AirportContract _airportContract;

        public Airport(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _airportContract = new AirportContract
            {
                Name = "Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString()
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_configuration
            //    .GetSection("Mqtt");
            //.Bind(quandlApiOptions);

            //TODO: move to app settings 
            var url = _hostEnvironment.EnvironmentName == "Docker"
                    ? $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdateAirportInfo"
                    : $"https://localhost:44389/api/airtrafficinfo/UpdateAirportInfo";

            while (!stoppingToken.IsCancellationRequested)
            {
                _airportContract.PositionX = new Random().Next(1, 100);

                await _httpClient.PostAsync(
                    url,
                    new StringContent(JsonConvert.SerializeObject(_airportContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }
    }
}
