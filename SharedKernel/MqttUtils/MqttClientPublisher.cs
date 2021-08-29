using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System.Threading.Tasks;

namespace MqttUtils
{
    /// <summary>
    /// Publishes messages to connected mqtt topic
    /// </summary>
    public class MqttClientPublisher : MqttClient
    {
        public MqttClientPublisher(IOptions<MqttConfig> config) : base(config)
        {

        }

        public async Task PublishAsync(string message, string topic)
        {
            var mqttApplicationMessageBuilder = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithExactlyOnceQoS()
                //.WithRetainFlag() no history of messages for planes during airport change will receive all messages from history
                .Build();

            //await _client.

            //await _client..PublishAsync(mqttApplicationMessageBuilder);
        }
    }
}
