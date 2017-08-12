
namespace RabbitMQSimpleConnector.Entity {

    /// <summary>
    /// Classe de configuração de conexão do RabbitMQ
    /// </summary>
    public sealed class ConnectionSetting {
        /// <summary>
        /// Descrição do servidor 
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// Nome do usuário do RabbitMQ
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Senha do RabbitMQ
        /// </summary>
        public string Password { get; set; }
    }
}
