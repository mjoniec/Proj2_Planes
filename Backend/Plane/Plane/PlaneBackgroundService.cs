using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public class PlaneBackgroundService : BackgroundService
    {
        private readonly IPlane _plane;

        public PlaneBackgroundService(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _plane = new Plane(configuration, hostEnvironment);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _plane.ExecuteAsync(stoppingToken);
        }
    }
}
