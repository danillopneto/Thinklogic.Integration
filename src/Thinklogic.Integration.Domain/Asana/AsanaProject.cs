using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Asana
{
    public class AsanaProject
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
