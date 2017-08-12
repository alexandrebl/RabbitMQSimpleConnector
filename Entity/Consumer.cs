using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSimpleConnector.Entity {
    /// <summary>
    /// Responsável por consumo de fila
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Consumer<T> : IDisposable {
        /// <summary>
        /// Canal de comunicação com a fila
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// Indica se a mensagem será removida ou não da fila
        /// </summary>
        private readonly bool _autoAck;

        /// <summary>
        /// Descrição da fila
        /// </summary>
        private readonly string _queueName;

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
        public Consumer(IModel channel, string queueName, ushort prefetchCount, bool autoAck) {
            this._channel = channel;
            this._autoAck = autoAck;
            this._queueName = queueName;
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

        /// <summary>
        /// Remove a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        public void Ack(ulong deliveryTag) {
            _channel.BasicAck(deliveryTag, false);
        }

        /// <summary>
        /// Retorna a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="requeued"></param>
        public void Nack(ulong deliveryTag, bool requeued = true) {
            _channel.BasicNack(deliveryTag, false, requeued);
        }

        /// <summary>
        /// 'IDisposable' implementation.
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 'IDisposable' implementation.
        /// </summary>
        /// <param name="disposeManaged">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposeManaged) {
            // Return if already disposed.
            if (this._alreadyDisposed) return;

            // Release managed resources if needed.
            if (disposeManaged) {
                this._channel.Dispose();
            }

            this._alreadyDisposed = true;
        }

        /// <summary>
        /// Whether the object was already disposed.
        /// </summary>
        private bool _alreadyDisposed = false;
    }
}
