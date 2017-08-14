using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace RabbitMQSimpleConnector.Library {
    [Serializable]
    public class ReceiveMessageException : Exception, _Exception {
        public string QueueMessage { get; set; }
        public object QueueType { get; set; }


        public ReceiveMessageException()
            : base() { }

        //public ReceiveMessageException(string message)
        //    : base(message) { }

        //public ReceiveMessageException(string format, params object[] args)
        //    : base(string.Format(format, args)) { }

        //public ReceiveMessageException(string message, Exception innerException)
        //    : base(message, innerException) { }

        //public ReceiveMessageException(string format, Exception innerException, params object[] args)
        //    : base(string.Format(format, args), innerException) { }

        //protected ReceiveMessageException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }


        

    }
}
