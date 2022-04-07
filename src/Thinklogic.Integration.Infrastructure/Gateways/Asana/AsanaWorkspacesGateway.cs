using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;
using Thinklogic.Integration.Domain.DataContracts;

namespace Thinklogic.Integration.Infrastructure.Gateways.Asana
{
    public class AsanaWorkspacesGateway : AsanaGateway<AsanaWorkspacesGateway>, IAsanaWorkspacesGateway
    {
        public AsanaWorkspacesGateway(IHttpClientFactory httpClientFactory,
                                      ILogger<AsanaWorkspacesGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "workspaces";
        }

        public async Task<AsanaTaskResponse> GetTaskAsync(string workspaceGid, string projectGid, string taskName, CancellationToken ct)
        {
            string url = $"{Client}/{workspaceGid}/tasks/search?projects.any={projectGid}&text={taskName}";
            var result = await SendGetRequest<AsanaData<IEnumerable<AsanaTaskResponse>>>(url, ct);

            return result.Data != null && result.Data.Any() ?
                   result.Data.First() :
                   default;
        }
    }
}
