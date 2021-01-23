using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AirTrafficInfoServices
{
    public abstract class AirTrafficBackgroundService : BackgroundService
    {
        protected readonly IAirTrafficService _service;

        protected AirTrafficBackgroundService(IAirTrafficService service)
        {
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.ExecuteAsync(stoppingToken);
        }
    }
}
