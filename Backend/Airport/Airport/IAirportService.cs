using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public interface IAirportService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
