using System.Threading.Tasks;

namespace Mqtt.Interfaces
{
    public interface IMqttClientPublisher : IMqttClient
    {
        Task PublishAsync(string message);
    }
}
