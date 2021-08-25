using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttUtils
{
    /// <summary>
    /// Receives messages from subscribed mqtt topic, and exposes event when message is received. 
    /// </summary>
    public class MqttClientSubscriber : MqttClient
    {
        public event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;

        public MqttClientSubscriber(IOptions<MqttConfig> config) : base(config)
        {
            _client.UseApplicationMessageReceivedHandler(async e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                OnRaiseMessageReceivedEvent(new MessageEventArgs(message));
            });
        }

        public async Task<bool> SubscribeToTopic(string topic)
        {
            var subscribeResults = await _client.SubscribeAsync(
                new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .Build());

            //TODO: improve this on if connected properly
            if (subscribeResults.Items.Any(item =>
                item.ResultCode == MQTTnet.Client.Subscribing.MqttClientSubscribeResultCode.UnspecifiedError))
            {
                return false;
            }

            return true;
        }

        public async Task Unsubscribe(string topic)
        {
            await _client.UnsubscribeAsync(topic);
        }

        private void OnRaiseMessageReceivedEvent(MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessageReceivedEvent;

            //no subscribers
            if (handler == null) return;

            //invokes handler set up by hosting service
            handler(this, e);
        }
    }
}
