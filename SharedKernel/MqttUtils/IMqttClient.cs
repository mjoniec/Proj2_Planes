using System.Threading.Tasks;

namespace MqttUtils
{
    public interface IMqttClient
    {
        Task<bool> Start();
    }
}
