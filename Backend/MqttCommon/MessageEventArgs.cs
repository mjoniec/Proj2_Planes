using System;

namespace MqttCommon
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
