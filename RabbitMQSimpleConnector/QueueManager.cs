using System;
using RabbitMQ.Client;
using RabbitMQSimpleConnector.Entity;
using RabbitMQSimpleConnector.Library;

namespace RabbitMQSimpleConnector {
    /// <summary>
    /// Responsável por gerenciar publicação e o consumo no RabbitMQ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueManager<T> : IDisposable {

        /// <summary>
        /// Canal de comunicação com a fila
        /// </summary>
        private IModel _channel;

        /// <summary>
        /// Produtor de mensagens
        /// </summary>
        public Producer<T> Producer { get; set; }

        /// <summary>
        /// Consumidor de mensagens
        /// </summary>
        public Consumer<T> Consumer { get; set; }

        /// <summary>
        /// Descrição da fila
        /// </summary>
        private readonly string _queueName;

        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="queueName">Descrição da fila</param>
        public QueueManager(string queueName = null) {
            _queueName = queueName;
        }

        /// <summary>
        /// Atribui uma configuração para conexão com RabbitMQ
        /// </summary>
        /// <param name="connectionSetting">Configurações de conexão</param>
        /// <returns>Instância de gerenciamento de fila com uma conexão com RabbitMQ</returns>
        public QueueManager<T> WithConnectionSetting(ConnectionSetting connectionSetting) {
            CreateChannel(connectionSetting);
            return this;
        }

        /// <summary>
        /// Cria uma canal de comunicação com RabbitMQ
        /// </summary>
        /// <param name="connectionSetting"></param>
        private void CreateChannel(ConnectionSetting connectionSetting) {
            _channel = ChannelFactory.Create(connectionSetting);
        }

        /// <summary>
        /// Atribui um consumidor ao gerenciador 
        /// </summary>
        /// <param name="prefetchCount">Controla o número de mensagem recebidas</param>
        /// <param name="autoAck">Indica se a mensagem será removida ou não da fila</param>
        /// <returns>Instância de gerenciamento de fila com um consumidor</returns>
        public QueueManager<T> WithConsumer(ushort prefetchCount = 1, bool autoAck = false) {
            if(_queueName == null) throw new Exception($"Queue name is undefined");
            
            this.Consumer = new Consumer<T>(_channel, _queueName, prefetchCount, autoAck);
            return this;
        }

        /// <summary>
        /// Atribui um produtor ao gerenciador
        /// </summary>
        /// <returns>Instância de gerenciamento de fila com um produtor</returns>
        public QueueManager<T> WithProducer() {

            this.Producer = new Producer<T>(_channel, _queueName);
            return this;
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
                this.Consumer?.Dispose();
                this.Producer?.Dispose();
                this._channel?.Dispose();
                ChannelFactory.CloseConnection();
            }

            this._alreadyDisposed = true;
        }

        /// <summary>
        /// Whether the object was already disposed.
        /// </summary>
        private bool _alreadyDisposed = false;
    }
}
