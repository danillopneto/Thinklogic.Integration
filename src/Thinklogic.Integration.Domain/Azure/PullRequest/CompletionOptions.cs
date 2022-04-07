using Newtonsoft.Json;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class CompletionOptions
    {
        [JsonProperty("mergeCommitMessage")]
        public string MergeCommitMessage { get; set; }

        [JsonProperty("deleteSourceBranch")]
        public bool DeleteSourceBranch { get; set; }

        [JsonProperty("mergeStrategy")]
        public string MergeStrategy { get; set; }

        [JsonProperty("transitionWorkItems")]
        public bool TransitionWorkItems { get; set; }
    }
}
