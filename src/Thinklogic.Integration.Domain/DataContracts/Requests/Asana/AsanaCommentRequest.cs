using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.DataContracts.Requests.Asana
{
    public class AsanaCommentRequest
    {
        [JsonProperty("html_text")]
        public string HtmlTextRequest => !HtmlText.Contains("<body>", StringComparison.InvariantCulture) ? $"<body>{HtmlText}</body>" : HtmlText;

        public string HtmlText { get; set; }

        [JsonProperty("is_pinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
