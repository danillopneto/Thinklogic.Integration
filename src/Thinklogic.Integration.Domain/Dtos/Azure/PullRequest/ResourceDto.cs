using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Dtos.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class ResourceDto
    {
        [JsonProperty("repository")]
        public RepositoryDto Repository { get; set; }

        [JsonProperty("pullRequestId")]
        public int PullRequestId { get; set; }

        [JsonProperty("codeReviewId")]
        public int CodeReviewId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdBy")]
        public PersonDto CreatedBy { get; set; }

        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("closedDate")]
        public DateTime ClosedDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sourceRefName")]
        public string SourceRefName { get; set; }

        [JsonProperty("targetRefName")]
        public string TargetRefName { get; set; }

        [JsonProperty("mergeStatus")]
        public string MergeStatus { get; set; }

        [JsonProperty("isDraft")]
        public bool IsDraft { get; set; }

        [JsonProperty("mergeId")]
        public string MergeId { get; set; }

        [JsonProperty("lastMergeCommit")]
        public LastMergeCommitDto LastMergeCommit { get; set; }

        [JsonProperty("reviewers")]
        public List<object> Reviewers { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("completionOptions")]
        public CompletionOptionsDto CompletionOptions { get; set; }

        [JsonProperty("supportsIterations")]
        public bool SupportsIterations { get; set; }

        [JsonProperty("completionQueueTime")]
        public DateTime CompletionQueueTime { get; set; }

        [JsonProperty("closedBy")]
        public PersonDto ClosedBy { get; set; }

        [JsonProperty("artifactId")]
        public string ArtifactId { get; set; }
    }
}
