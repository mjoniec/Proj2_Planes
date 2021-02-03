using System.Threading.Tasks;

namespace MqttCommon
{
    public interface IMqttClient
    {
        Task<bool> Start();
    }
}
