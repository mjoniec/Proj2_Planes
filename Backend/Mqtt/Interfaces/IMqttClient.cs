using System.Threading.Tasks;

namespace Mqtt.Interfaces
{
    public interface IMqttClient
    {
        Task<bool> Start();
    }
}
