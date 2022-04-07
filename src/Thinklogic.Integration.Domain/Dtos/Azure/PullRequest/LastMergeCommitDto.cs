using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Dtos.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class LastMergeCommitDto
    {
        [JsonProperty("commitId")]
        public string CommitId { get; set; }

        [JsonProperty("author")]
        public BasicPersonDto Author { get; set; }

        [JsonProperty("committer")]
        public BasicPersonDto Committer { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("commentTruncated")]
        public bool CommentTruncated { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
