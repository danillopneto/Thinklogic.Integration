using Newtonsoft.Json;
using System.Dynamic;

namespace Thinklogic.Integration.Domain.DataContracts.Requests.Asana
{
    public class AsanaCustomFieldRequest
    {
        [JsonProperty("custom_fields")]
        public ExpandoObject CustomFields { get; set; }

        public AsanaCustomFieldRequest(string key, string value)
        {
            CustomFields = new ExpandoObject();
            CustomFields.TryAdd(key, value);
        }
    }
}
