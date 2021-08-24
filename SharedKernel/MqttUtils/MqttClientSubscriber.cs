using Microsoft.Extensions.Options;
using MQTTnet.Client;
using System;
using System.Text;

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
