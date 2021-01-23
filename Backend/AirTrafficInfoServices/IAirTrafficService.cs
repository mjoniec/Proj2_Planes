using System.Threading;
using System.Threading.Tasks;

namespace AirTrafficInfoServices
{
    public interface IAirTrafficService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
