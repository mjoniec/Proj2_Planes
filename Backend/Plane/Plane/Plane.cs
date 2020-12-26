using AirTrafficInfoContracts;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public class Plane : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly PlaneContract _planeContract;

        public Plane()
        {
            _httpClient = new HttpClient();
            _planeContract = new PlaneContract
            {
                Name = "Name_" + new Random().Next(1, 100).ToString()
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _planeContract.PositionX = new Random().Next(1, 100);

                await _httpClient.PostAsync(
                    $"https://localhost:44389/api/airtrafficinfo",
                    new StringContent(JsonConvert.SerializeObject(_planeContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
