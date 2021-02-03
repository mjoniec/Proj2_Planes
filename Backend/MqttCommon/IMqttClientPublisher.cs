using System.Threading.Tasks;

namespace MqttCommon
{
    public interface IMqttClientPublisher : IMqttClient
    {
        Task PublishAsync(string message);
    }
}
