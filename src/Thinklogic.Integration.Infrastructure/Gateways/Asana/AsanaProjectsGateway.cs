using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Domain.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

namespace Thinklogic.Integration.Infrastructure.Gateways.Asana
{
    public class AsanaProjectsGateway : AsanaGateway<AsanaProjectsGateway>, IAsanaProjectsGateway
    {
        public AsanaProjectsGateway(IHttpClientFactory httpClientFactory,
                                    ILogger<AsanaProjectsGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "projects";
        }

        public async Task<IEnumerable<AsanaProject>> GetProjectsAsync(string workspaceGid, CancellationToken ct)
        {
            string url = $"{Client}?opt_fields=gid,name&archived=false&workspace={workspaceGid}";
            var result = await SendGetRequest<AsanaResponse<IEnumerable<AsanaProject>>>(url, ct);
            return result.Data;
        }
    }
}
