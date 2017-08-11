using RabbitMQ.Client;
using RabbitMQSimpleConnector.Entity;

namespace RabbitMQSimpleConnector.Library {
    /// <summary>
    /// Responsável por criar conexões com RabbitMQ
    /// </summary>
    public static class ChannelFactory {
        /// <summary>
        /// Objeto
        /// </summary>
        private static readonly object SyncObj = new object();

        /// <summary>
        /// Interface de conexão com RabbitMQ
        /// </summary>
        private static IConnection _connection;

        /// <summary>
        /// Método cria uma conexão com RabbitMQ
        /// </summary>
        /// <param name="connectionConfig">Configurações de conexão</param>
        /// <returns></returns>
        public static IModel Create(ConnectionSetting connectionConfig) {
            var factory = new ConnectionFactory {
                HostName = connectionConfig.HostName,
                VirtualHost = connectionConfig.VirtualHost,
                UserName = connectionConfig.UserName,
                Password = connectionConfig.Password,
                AutomaticRecoveryEnabled = true
            };

            if (_connection == null) {
                lock (SyncObj) {
                    if (_connection == null) {
                        _connection = factory.CreateConnection();
                    }
                }
            }

            var channel = _connection.CreateModel();

            return channel;
        }

        /// <summary>
        /// Método responsável por fechar a conexão com RabbitMQ
        /// </summary>
        public static void CloseConnection() {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
    }
}
