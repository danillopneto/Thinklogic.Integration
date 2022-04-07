using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class MessageDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("markdown")]
        public string Markdown { get; set; }
    }
}
