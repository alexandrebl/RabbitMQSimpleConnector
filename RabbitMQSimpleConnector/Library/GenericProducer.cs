using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnector.Entity;

namespace RabbitMQSimpleConnector.Library {
    public class GenericProducer
    {
        private readonly IModel _channel;

        public GenericProducer(ConnectionSetting connectionSetting)
        {
            _channel = ChannelFactory.Create(connectionSetting);
        }

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public void Publish<T>(T obj, string exchange = null, string routingKey = null, IBasicProperties basicProperties = null) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);

            _channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: basicProperties, body: buffer);
        }
    }
}
