using Newtonsoft.Json;

namespace Identity.Integration.Utils.Kafka.Models
{
    internal class ParrotCreatedData
    {
        [JsonProperty("parrotPid"), JsonRequired]
        public string PublicId { get; set; }

        [JsonProperty("nickname"), JsonRequired]
        public string Nickname { get; set; }

        [JsonProperty("email"), JsonRequired]
        public string Email { get; set; }

        [JsonProperty("role"), JsonRequired]
        public string RolePid { get; set; }

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
