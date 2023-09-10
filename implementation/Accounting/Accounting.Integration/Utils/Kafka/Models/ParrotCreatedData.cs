using Newtonsoft.Json;

namespace Accounting.Integration.Utils.Kafka.Models
{
    internal class ParrotCreatedData
    {
        [JsonProperty("parrotPid"), JsonRequired]
        public string PublicId { get; set; }

        [JsonProperty("role"), JsonRequired]
        public string RolePid { get; set; }

        [JsonProperty("email"), JsonRequired]
        public string Email { get; set; }

        [JsonProperty("personalAccountInfo"), JsonRequired]
        public PersonalAccountInfo PersonalAccountInfo { get; set; }
    }

    internal class PersonalAccountInfo
    {
        [JsonProperty("accountNumber"), JsonRequired]
        public string AccountNumber { get; set; }

        [JsonProperty("accountNickname"), JsonRequired]
        public string AccountNickname { get; set; }
    }
}
