using System;
using RabbitMQ.Client;

namespace RabbitMQSimpleConnector.Library.Base {
    public abstract class BaseQueue: IDisposable {

        /// <summary>
        /// Canal de comunicação com a fila
        /// </summary>
        internal readonly IModel Channel;

        /// <summary>
        /// Descrição da fila
        /// </summary>
        internal readonly string QueueName;

        /// <summary>
        /// Whether the object was already disposed.
        /// </summary>
        private bool _alreadyDisposed = false;

        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="channel">Indica se a mensagem será removida ou não da fila</param>
        /// <param name="queueName">Descrição da fila</param>
        protected BaseQueue(IModel channel, string queueName) {
            Channel = channel;
            QueueName = queueName;
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
                this.Channel.Dispose();
            }

            this._alreadyDisposed = true;
        }
    }
}
