using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class Collection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }
    }
}
