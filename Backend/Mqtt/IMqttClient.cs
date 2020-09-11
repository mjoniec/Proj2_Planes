using System;
using System.Threading.Tasks;

namespace Mqtt
{
    public interface IMqttClient
    {
        event EventHandler<MessageEventArgs> RaiseMessageReceivedEvent;
        Task<bool> Start();
        void Send(string message);
    }
}
