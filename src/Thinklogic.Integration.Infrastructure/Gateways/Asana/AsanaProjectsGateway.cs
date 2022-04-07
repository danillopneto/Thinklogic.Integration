using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;
using Thinklogic.Integration.Domain.DataContracts;

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

        public async Task<IEnumerable<AsanaProjectResponse>> GetProjectsAsync(string workspaceGid, CancellationToken ct)
        {
            string url = $"{Client}?opt_fields=gid,name&archived=false&workspace={workspaceGid}";
            var result = await SendGetRequest<AsanaData<IEnumerable<AsanaProjectResponse>>>(url, ct);
            return result.Data;
        }
    }
}
