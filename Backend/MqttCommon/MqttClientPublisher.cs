using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;

namespace MqttCommon
{
    /// <summary>
    /// Publishes messages to connected mqtt topic
    /// </summary>
    public class MqttClientPublisher : MqttClient, IMqttClientPublisher
    {
        //public MqttClientPublisher(MqttConfig config) : base(config)
        public MqttClientPublisher(IOptions<MqttConfig> config) : base(config)
        {

        }

        async Task IMqttClientPublisher.PublishAsync(string message)
        {
            var mqttApplicationMessageBuilder = new MqttApplicationMessageBuilder()
                .WithTopic(_config.Value.Topic)
                .WithPayload(message)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _client.PublishAsync(mqttApplicationMessageBuilder);
        }
    }
}
