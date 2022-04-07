using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class Repository
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("project")]
        public AzureProject Project { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("remoteUrl")]
        public string RemoteUrl { get; set; }

        [JsonProperty("sshUrl")]
        public string SshUrl { get; set; }

        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }
    }
}
