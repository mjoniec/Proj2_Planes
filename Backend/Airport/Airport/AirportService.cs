using AirTrafficInfoContracts;
using AirTrafficInfoServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public class AirportService : IAirTrafficService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly AirportContract _airportContract;

        public AirportService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var color = configuration.GetValue<string>("color");//required to install nuget: Microsoft.Extensions.Configuration.Binder
            var latitude = configuration.GetValue<string>("latitude");
            var longitude = configuration.GetValue<string>("longitude");

            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _airportContract = new AirportContract
            {
                Name = "Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString(),
                Color = string.IsNullOrEmpty(color) ? "#" + new Random().Next(100000, 999999).ToString() : color,
                Latitude = string.IsNullOrEmpty(latitude) ? new Random().Next(-150, 170) : Convert.ToDouble(latitude),
                Longitude = string.IsNullOrEmpty(longitude) ? new Random().Next(-55, 70) : Convert.ToDouble(longitude)
            };
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
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
                await _httpClient.PostAsync(
                    url,
                    new StringContent(JsonConvert.SerializeObject(_airportContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(1100, stoppingToken);
            }
        }
    }
}
