using RabbitMQ.Client;
using RabbitMQSimpleConnector.Entity;


namespace RabbitMQSimpleConnector.Library {
    public static class ChannelFactory {

        private static readonly object SyncObj = new object();

        private static IConnection _connection;

        public static IModel Create(ConnectionConfig connectionConfig) {

            var factory = new ConnectionFactory() {
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

        private static void CloseConnection()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
