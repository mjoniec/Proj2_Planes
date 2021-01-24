using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MockAirTrafficinfoApi.Services
{
    public class MockAirTrafficInfoBackgroundService : BackgroundService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly HttpClient _httpClient;

        public MockAirTrafficInfoBackgroundService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var url = _hostEnvironment.EnvironmentName == "Development"
                ? $"https://localhost:44389/api/mockAirTrafficInfo"
                : "";//todo azure

            while (!stoppingToken.IsCancellationRequested)
            {
                await _httpClient.PostAsync(url, null);

                await Task.Delay(1100, stoppingToken);
            }
        }
    }
}
