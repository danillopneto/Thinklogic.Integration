using Newtonsoft.Json;
using Thinklogic.Integration.Domain.Azure.PullRequest;

namespace Thinklogic.Integration.Domain.Azure.PullRequest
{
    public class PullRequest
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
        public Message Message { get; set; }

        [JsonProperty("detailedMessage")]
        public Message DetailedMessage { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
