using Newtonsoft.Json;

namespace TaskManager.Integration.Utils.Kafka.Models
{
    internal class ParrotChangedData
    {
        [JsonProperty("parrotPid"), JsonRequired]
        public string PublicId { get; set; }

        [JsonProperty("role"), JsonRequired]
        public string RolePid { get; set; }
    }
}
