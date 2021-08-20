using System.Threading.Tasks;

namespace MqttUtils
{
    public interface IMqttClientPublisher : IMqttClient
    {
        Task PublishAsync(string message);
    }
}
