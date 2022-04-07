
using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.DataContracts.Responses.Asana
{
    public class AsanaProjectResponse
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
