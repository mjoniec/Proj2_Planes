using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MqttUtils
{
    public abstract class MqttClient
    {
        //protected readonly MqttConfig _config;
        protected readonly IOptions<MqttConfig> _config;
        protected readonly IMqttClient _client = new MqttFactory().CreateMqttClient();

        //public MqttClient(MqttConfig config)
        public MqttClient(IOptions<MqttConfig> config)
        {
            _config = config;
        }

        public async Task<bool> Start()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_config.Value.Ip, _config.Value.Port)
                .Build();

            await _client.ConnectAsync(options);

            var subscribeResults = await _client
                .SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(_config.Value.Topic)
                .Build());

            //TODO: improve this on if connected properly
            if (subscribeResults.Items.Any(item =>
                item.ResultCode == MQTTnet.Client.Subscribing.MqttClientSubscribeResultCode.UnspecifiedError))
            {
                return false;
            }

            return true;
        }
    }
}
