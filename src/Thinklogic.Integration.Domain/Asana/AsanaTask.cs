using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Asana
{
    public class AsanaTask
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
