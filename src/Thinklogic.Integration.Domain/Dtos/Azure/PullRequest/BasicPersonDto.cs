using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class BasicPersonDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
