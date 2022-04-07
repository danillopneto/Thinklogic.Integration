using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class AzureProjectDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("revision")]
        public int Revision { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }
    }
}
