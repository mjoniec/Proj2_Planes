using System.Threading;
using System.Threading.Tasks;

namespace Airport
{
    public interface IAirport
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
