using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQSimpleConnector.Library {
    public static class GenericProducer<T> {

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public static void GenericPublish(T obj, IModel channel = null, string exchange = null, string routingKey = null) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);

            channel.BasicPublish(exchange: exchange ?? "", routingKey: routingKey, basicProperties: null, body: buffer);
        }
    }
}
