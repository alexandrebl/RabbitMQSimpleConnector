namespace RabbitMQSimpleConnector.Domain {
    public sealed class PrepareConfiguration {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RouteName { get; set; }
        public string DeadLetterRouteName { get; set; }
        public bool Lazy { get; set; }
        public sbyte? MaxPriority { get; set; }
    }
}
