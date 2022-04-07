using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class CollectionDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }
    }
}
