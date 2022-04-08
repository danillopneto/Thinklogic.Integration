using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.DataContracts.Responses.Asana
{
    public class AsanaCustomFieldResponse
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enum_options")]
        public List<AsanaCustomFieldOptionResponse> EnumOptions { get; set; }
    }
}
