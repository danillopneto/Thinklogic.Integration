using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Domain.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

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

        public async Task<AsanaTask> GetTaskAsync(string workspaceGid, string projectGid, string taskName, CancellationToken ct)
        {
            string url = $"{Client}/{workspaceGid}/tasks/search?projects.any={projectGid}&text={taskName}";
            var result = await SendGetRequest<AsanaResponse<IEnumerable<AsanaTask>>>(url, ct);

            return result.Data != null && result.Data.Any() ?
                   result.Data.First() :
                   default;
        }
    }
}
