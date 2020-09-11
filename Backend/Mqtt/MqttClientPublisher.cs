using Mqtt.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;

namespace Mqtt
{
    public class MqttClientPublisher : MqttClient, IMqttClientPublisher
    {
        public MqttClientPublisher(MqttConfig config) : base(config)
        {

        }

        async Task IMqttClientPublisher.PublishAsync(string message)
        {
            var mqttApplicationMessageBuilder = new MqttApplicationMessageBuilder()
                .WithTopic(_config.Topic)
                .WithPayload(message)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _client.PublishAsync(mqttApplicationMessageBuilder);
        }
    }
}
