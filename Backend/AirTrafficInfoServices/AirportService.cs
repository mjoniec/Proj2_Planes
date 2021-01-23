﻿using AirTrafficInfoContracts;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirTrafficInfoServices
{
    public class AirportService : IAirTrafficService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly AirportContract _airportContract;

        public AirportService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
            _airportContract = new AirportContract
            {
                Name = "Airport_" + _hostEnvironment.EnvironmentName + "_" + new Random().Next(1001, 9999).ToString(),
                Color = "#" + new Random().Next(100000, 999999).ToString()
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

            _airportContract.Longitude = new Random().Next(1, 100);
            _airportContract.Latitude = new Random().Next(1, 100);

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