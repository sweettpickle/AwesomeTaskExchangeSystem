namespace TaskManager.Integration.Utils.Kafka
{
    internal class KafkaConfig
    {
        public Uri SchemaRegistryUrl { get; set; }
        public string BootstrapServers { get; set; }
        public string ParrotsStreamingTopic { get; set; }
        public string ParrotsBusinessEventsTopic { get; set; }
        public string TasksStreamingTopic { get; set; }
        public string TasksBusinessEventsTopic { get; set; }
    }
}
