using Microsoft.Extensions.Logging;
using Thinklogic.Integration.Domain.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

namespace Thinklogic.Integration.Infrastructure.Gateways.Asana
{
    public class AsanaProjectsGateway : BaseGateway<AsanaProjectsGateway>, IAsanaProjectsGateway
    {
        protected override string BaseClient => "Asana";

        public AsanaProjectsGateway(IHttpClientFactory httpClientFactory,
                               ILogger<AsanaProjectsGateway> logger)
            : base(httpClientFactory, logger)
        {
            Client = "projects";
        }

        public async Task<IEnumerable<AsanaProject>> GetProjectsAsync(long asanaWorkspaceGid, CancellationToken ct)
        {
            string url = $"{Client}?opt_fields=gid,name&archived=false&workspace={asanaWorkspaceGid}";
            return await SendGetRequest<IEnumerable<AsanaProject>>(url, ct);
        }
    }
}
