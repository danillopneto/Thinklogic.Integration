using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class LastMergeCommit
    {
        [JsonProperty("commitId")]
        public string CommitId { get; set; }

        [JsonProperty("author")]
        public BasicPerson Author { get; set; }

        [JsonProperty("committer")]
        public BasicPerson Committer { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("commentTruncated")]
        public bool CommentTruncated { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
