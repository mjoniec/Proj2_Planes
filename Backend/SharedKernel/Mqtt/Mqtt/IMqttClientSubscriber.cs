using System;

namespace Mqtt
{
    public interface IMqttClientSubscriber : IMqttClient
    {
        event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;
    }
}
