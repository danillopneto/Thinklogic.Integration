using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.DataContracts.Responses.Asana
{
    public class AsanaCommentResponse
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("is_pinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
