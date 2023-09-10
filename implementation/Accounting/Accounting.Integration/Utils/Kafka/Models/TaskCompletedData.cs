using Newtonsoft.Json;

namespace Accounting.Integration.Utils.Kafka.Models
{
    internal class TaskCompletedData
    {
        [JsonProperty("taskPid"), JsonRequired]
        public string TaskPid { get; set; }

        [JsonProperty("parrotPid"), JsonRequired]
        public string ParrotPid { get; set; }

        [JsonProperty("amount"), JsonRequired]
        public decimal Amount { get; set; }
    }
}
