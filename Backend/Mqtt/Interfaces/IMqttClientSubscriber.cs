using System;

namespace Mqtt.Interfaces
{
    public interface IMqttClientSubscriber : IMqttClient
    {
        event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;//is this required ?
    }
}
