using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly IPlaneService _service;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _service = new PlaneService(configuration, hostEnvironment);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.ExecuteAsync(stoppingToken);
        }
    }
}
