using System.Linq;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
//using MQTTnet.Extensions.ManagedClient; TODO: test if client works - if so delete, if not then look at this, may have been needed in previous version impl

namespace Mqtt
{
    /// <summary>
    /// Publishes and receives messages from subscribed mqtt topic
    /// </summary>
    public abstract class MqttClient : Interfaces.IMqttClient
    {
        protected readonly MqttConfig _config;
        protected readonly IMqttClient _client = new MqttFactory().CreateMqttClient();

        public MqttClient(MqttConfig config)
        {
            _config = config;
        }

        public async Task<bool> Start()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_config.Ip, _config.Port)
                .Build();

            await _client.ConnectAsync(options);

            var subscribeResults = await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(_config.Topic).Build());

            //TODO: improve this on if connected properly
            if (subscribeResults.Items.Any(r => r.ResultCode == MQTTnet.Client.Subscribing.MqttClientSubscribeResultCode.UnspecifiedError)) return false;

            return true;
        }
    }
}
