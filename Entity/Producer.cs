using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQSimpleConnector.Entity {
    /// <summary>
    /// Responsável por produzir mensagens na fila
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Producer<T> : IDisposable {
        /// <summary>
        /// Canal de comunicação com a fila
        /// </summary>
        private readonly IModel _channel;

        /// <summary>
        /// Descrição da fila
        /// </summary>
        private readonly string _queueName;

        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="channel">Canal de comunicação com a fila</param>
        /// <param name="queueName">Descrição da fila</param>
        public Producer(IModel channel, string queueName) {
            this._channel = channel;
            this._queueName = queueName;
        }

        /// <summary>
        /// Publica a mensagem na fila
        /// </summary>
        /// <param name="obj"></param>
        public void Publish(T obj) {
            var data = JsonConvert.SerializeObject(obj);
            var buffer = Encoding.UTF8.GetBytes(data);
            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: buffer);
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
