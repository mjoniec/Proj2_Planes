using Mqtt.Interfaces;
using MQTTnet.Client;
using System;
using System.Text;

namespace Mqtt
{
    public class MqttClientSubscriber : MqttClient, IMqttClientSubscriber
    {
        public event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;

        public MqttClientSubscriber(MqttConfig config) : base(config)
        {
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
    }
}
