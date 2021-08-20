using System;

namespace MqttUtils
{
    public interface IMqttClientSubscriber : IMqttClient
    {
        event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;
    }
}
