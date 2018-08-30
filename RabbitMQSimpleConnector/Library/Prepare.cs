using RabbitMQ.Client;
using RabbitMQSimpleConnector.Domain;
using System.Collections.Generic;

namespace RabbitMQSimpleConnector.Library {
    public static class Prepare {

        public static void ExchangeDeclare(IModel channel, string exchange, string type = "topic", bool durable = true)
        {
            channel.ExchangeDeclare(exchange, type, true);
        }

        public static void QueueDeclareAndBind(IModel channel, PrepareConfiguration prepareConfiguration, bool autoDelete = false, bool exclusive = false) {
            var queueArguments = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(prepareConfiguration.DeadLetterRouteName)) {
                queueArguments.Add("x-dead-letter-exchange", prepareConfiguration.ExchangeName);
                queueArguments.Add("x-dead-letter-routing-key", prepareConfiguration.DeadLetterRouteName);
            }

            if (prepareConfiguration.Lazy) {
                queueArguments.Add("x-queue-mode", "lazy");
            }

            if (prepareConfiguration.MaxPriority != null) {
                queueArguments.Add("x-max-priority", prepareConfiguration.MaxPriority.Value);
            }

            QueueDeclare(channel, prepareConfiguration, queueArguments, autoDelete);
            QueueBind(channel, prepareConfiguration);
        }

        public static void QueueDeclare(IModel channel, PrepareConfiguration prepareConfiguration,
            IDictionary<string, object> queueArguments, bool autoDelete = false, bool exclusive = false) {
            channel.QueueDeclare(prepareConfiguration.QueueName, arguments: queueArguments, autoDelete: autoDelete, exclusive: exclusive);
        }

        public static void QueueBind(IModel channel, PrepareConfiguration prepareConfiguration) {
            channel.QueueBind(prepareConfiguration.QueueName, prepareConfiguration.ExchangeName, prepareConfiguration.RouteName);
        }
    }
}
