using System.Threading.Tasks;

namespace Mqtt
{
    public interface IMqttClientPublisher : IMqttClient
    {
        Task PublishAsync(string message);
    }
}
