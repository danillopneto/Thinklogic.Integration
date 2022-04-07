using Thinklogic.Integration.Domain.Asana;

namespace Thinklogic.Integration.Interfaces.Gateways.Asana
{
    public interface IAsanaProjectsGateway
    {
        Task<IEnumerable<AsanaProject>> GetProjectsAsync(string workspaceGid, CancellationToken ct);
    }
}
