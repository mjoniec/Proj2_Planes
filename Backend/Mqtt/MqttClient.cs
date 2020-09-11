using System;
using System.Linq;
using System.Text;
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
    public class MqttClient : IMqttClient
    {
        private readonly MqttConfig _config;
        private readonly MQTTnet.Client.IMqttClient _client = new MqttFactory().CreateMqttClient();

        public event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;

        public MqttClient(MqttConfig config)
        {
            _config = config;
            _client.UseApplicationMessageReceivedHandler(async e =>
            {
                {
                    var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                    OnRaiseMessageReceivedEvent(new MessageEventArgs(message));
                };
            });
        }

        private void OnRaiseMessageReceivedEvent(MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessageReceivedEvent;

            //no subscribers
            if (handler == null) return;

            handler(this, e);
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

        public async void Send(string message)
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
