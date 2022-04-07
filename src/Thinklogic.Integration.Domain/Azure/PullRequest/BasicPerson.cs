using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class BasicPerson
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
