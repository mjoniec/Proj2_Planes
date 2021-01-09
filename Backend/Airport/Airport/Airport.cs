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
                Name = "Airport_" + new Random().Next(1001, 9999).ToString()
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tst = 

            _configuration
                .GetSection("Mqtt");
                //.Bind(quandlApiOptions);

            //dev/docker if before loop

            while (!stoppingToken.IsCancellationRequested)
            {
                _airportContract.PositionX = new Random().Next(1, 100);

                //_configuration.

                await _httpClient.PostAsync(
                    //TODO: refactor to appsettings dev and docker
                    //$"https://localhost:44389/api/airtrafficinfo/UpdateAirportInfo",
                    $"http://airtrafficinfo_1:80/api/airtrafficinfo/UpdateAirportInfo",
                    new StringContent(JsonConvert.SerializeObject(_airportContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }
    }
}
