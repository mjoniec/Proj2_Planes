using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public interface IPlaneService
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
