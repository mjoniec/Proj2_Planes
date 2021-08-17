using System.Threading.Tasks;

namespace Mqtt
{
    public interface IMqttClient
    {
        Task<bool> Start();
    }
}
