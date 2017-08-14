using RabbitMQ.Client;
using RabbitMQSimpleConnector.Library.Base;

namespace RabbitMQSimpleConnector.Library {
    public class Rpc : BaseQueue {
        public Rpc(IModel channel, string queueName) 
            : base(channel, queueName)
        {
        }
    }
}
