using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.DataContracts.Responses.Asana
{
    public class AsanaCustomFieldOptionResponse
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
