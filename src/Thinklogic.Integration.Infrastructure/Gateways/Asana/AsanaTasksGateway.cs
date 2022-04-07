using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Domain.DataContracts;
using Thinklogic.Integration.Domain.DataContracts.Requests.Asana;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

namespace Thinklogic.Integration.Infrastructure.Gateways.Asana
{
    public class AsanaTasksGateway : AsanaGateway<AsanaTasksGateway>, IAsanaTasksGateway
    {
        public AsanaTasksGateway(IHttpClientFactory httpClientFactory,
                                 ILogger<AsanaTasksGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "tasks";
        }

        public async Task<AsanaCommentResponse> IncludeCommentAsync(string taskGid, AsanaCommentRequest comment)
        {
            var url = $"{Client}/{taskGid}/stories";
            var payload = new AsanaData<AsanaCommentRequest> { Data = comment };
            var result = await SendPostRequest<AsanaData<AsanaCommentRequest>, AsanaData<AsanaCommentResponse>>(url, payload);
            return result.Data;
        }
    }
}
