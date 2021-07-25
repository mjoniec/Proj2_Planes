using System.Threading;
using System.Threading.Tasks;

namespace Plane
{
    public interface IPlane
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
