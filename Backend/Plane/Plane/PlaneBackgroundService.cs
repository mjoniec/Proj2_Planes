using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly Plane _plane;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _plane = new Plane(configuration, hostEnvironment);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await _plane.ExecuteAsync(stoppingToken);

            await _plane.StartPlane();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdatePlane();

                await _httpClient.PostAsync(
                    AirTrafficApiUpdatePlaneInfoUrl,
                    new StringContent(JsonConvert.SerializeObject(_planeContract),
                    Encoding.UTF8, "application/json"));

                await Task.Delay(600, stoppingToken);
            }
        }
    }
}
