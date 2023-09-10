using Newtonsoft.Json;

namespace Identity.Integration.Utils.Kafka
{
    internal class MessageBase<T>
    {
        [JsonProperty("messageType"), JsonRequired]
        public string MessageType { get; set; }

        [JsonProperty("data"), JsonRequired]
        public T Data { get; set; }
    }
}
