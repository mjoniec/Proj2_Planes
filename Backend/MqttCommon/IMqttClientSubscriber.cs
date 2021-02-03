using System;

namespace MqttCommon
{
    public interface IMqttClientSubscriber : IMqttClient
    {
        event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;
    }
}
