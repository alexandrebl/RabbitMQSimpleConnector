using System;
using System.Runtime.InteropServices;

namespace RabbitMQSimpleConnector.Library {
    [Serializable]
    public class ReceiveMessageException : Exception, _Exception {
        public string QueueMessage { get; set; }
        public object QueueType { get; set; }


        public ReceiveMessageException()
            : base() { }
    }
}
