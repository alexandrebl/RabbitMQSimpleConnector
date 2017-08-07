using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQSimpleConnector.Entity;
using RabbitMQSimpleConnector.Library;

namespace RabbitMQSimpleConnector {
    public class QueueManager<T> {

        private readonly IModel _channel;
        private readonly bool _autoAck;
        private readonly string _queueName;
        private readonly ushort _prefetchCount;

        public event Action<T, ulong> ReceiveMessage;

        public QueueManager(ConnectionConfig connectionConfig, string queueName, ushort prefetchCount = 1, bool autoAck = false) {
            _queueName = queueName;
            _autoAck = autoAck;
            _prefetchCount = prefetchCount;
            _channel = ChannelFactory.Create(connectionConfig);
        }

        public void WatchInit() {
            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) => {
                var body = ea.Body; 
                 var message = Encoding.UTF8.GetString(body);

                var data = JsonConvert.DeserializeObject<T>(message);
                    
                ReceiveMessage?.Invoke(data, ea.DeliveryTag);
                
            };

            _channel.BasicQos(0, _prefetchCount, false);

            _channel.BasicConsume(queue: _queueName, autoAck: _autoAck, consumer: consumer);
        }

        public void Publish(T obj)
        {
            var data = JsonConvert.SerializeObject(obj);

            var buffer = Encoding.UTF8.GetBytes(data);

            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: buffer);
        }

        public void Ack(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag, false);
        }

        public void NAck(ulong deliveryTag, bool requeued = true) {
            _channel.BasicNack(deliveryTag, false, requeued);
        }
    }
}
