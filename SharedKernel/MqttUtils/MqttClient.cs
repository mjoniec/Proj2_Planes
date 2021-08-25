using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace MqttUtils
{
    public abstract class MqttClient
    {
        protected readonly IOptions<MqttConfig> _config;
        protected readonly IMqttClient _client = new MqttFactory().CreateMqttClient();

        public MqttClient(IOptions<MqttConfig> config)
        {
            _config = config;
        }

        public async Task Start()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_config.Value.Ip, _config.Value.Port)
                .Build();

            await _client.ConnectAsync(options);
        }
    }
}
