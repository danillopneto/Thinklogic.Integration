using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Dtos.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Dtos.Azure.PullRequest
{
    public class PullRequestDto
    {
        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("notificationId")]
        public int NotificationId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("publisherId")]
        public string PublisherId { get; set; }

        [JsonProperty("message")]
        public MessageDto Message { get; set; }

        [JsonProperty("detailedMessage")]
        public MessageDto DetailedMessage { get; set; }

        [JsonProperty("resource")]
        public ResourceDto Resource { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
