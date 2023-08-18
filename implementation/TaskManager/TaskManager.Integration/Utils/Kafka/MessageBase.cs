using Confluent.Kafka;

namespace TaskManager.Integration.Utils.Kafka
{
    public class MessageBase<T> 
    {
        public string MessageType { get; set; }
        public T Data { get; set; }
    }
}
