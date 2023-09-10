using Newtonsoft.Json;

namespace Identity.Integration.Utils.Kafka.Models
{
    internal class ParrotRoleChangedData
    {
        [JsonProperty("parrotPid"), JsonRequired]
        public string PublicId { get; set; }

        [JsonProperty("role"), JsonRequired]
        public string RolePid { get; set; }
    }
}
