using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQSimpleConnector.Library.Base;

namespace RabbitMQSimpleConnector.Library {
    /// <summary>
    /// Responsável por produzir mensagens na fila
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Producer<T> : BaseQueue {

        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="channel">Canal de comunicação com a fila</param>
        /// <param name="queueName">Descrição da fila</param>
        public Producer(IModel channel, string queueName)
            : base(channel, queueName) { }

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public void Publish(T obj) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);
            this.Channel.BasicPublish(exchange: "", routingKey: this.QueueName, basicProperties: null, body: buffer);
        }
    }
}
