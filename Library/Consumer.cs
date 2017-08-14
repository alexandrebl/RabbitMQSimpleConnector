using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQSimpleConnector.Library.Base;

namespace RabbitMQSimpleConnector.Library {
    /// <summary>
    /// Responsável por consumo de fila
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Consumer<T> : BaseQueue {

        /// <summary>
        /// Indica se a mensagem será removida ou não da fila
        /// </summary>
        private readonly bool _autoAck;

        /// <summary>
        /// Controla o número de mensagem recebidas 
        /// </summary>
        private readonly ushort _prefetchCount;

        /// <summary>
        /// Evento de recebimento da mensagem
        /// </summary>
        public event Action<T, ulong> ReceiveMessage;

        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="channel">Indica se a mensagem será removida ou não da fila</param>
        /// <param name="queueName">Descrição da fila</param>
        /// <param name="prefetchCount">Controla o número de mensagem recebidas</param>
        /// <param name="autoAck">Indica se a mensagem será removida ou não da fila</param>
        public Consumer(IModel channel, string queueName, ushort prefetchCount, bool autoAck)
            : base(channel, queueName) {
            this._autoAck = autoAck;
            this._prefetchCount = prefetchCount;
        }

        /// <summary>
        /// Inicia o consumidor e fica aguardando mensagens
        /// </summary>
        public void WatchInit() {
            this.InitializeObject();
        }

        /// <summary>
        /// Cria o objeto consumidor
        /// </summary>
        private void InitializeObject() {
            Channel.QueueDeclare(QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (model, ea) => {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                var data = JsonConvert.DeserializeObject<T>(message);

                ReceiveMessage?.Invoke(data, ea.DeliveryTag);
            };

            Channel.BasicQos(0, _prefetchCount, false);

            Channel.BasicConsume(queue: QueueName, autoAck: _autoAck, consumer: consumer);
        }

        /// <summary>
        /// Remove a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        public void Ack(ulong deliveryTag) {
            Channel.BasicAck(deliveryTag, false);
        }

        /// <summary>
        /// Retorna a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="requeued"></param>
        public void Nack(ulong deliveryTag, bool requeued = true) {
            Channel.BasicNack(deliveryTag, false, requeued);
        }
    }
}
