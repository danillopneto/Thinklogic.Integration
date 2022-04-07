using Thinklogic.Integration.Domain.Asana;

namespace Thinklogic.Integration.Interfaces.Gateways.Asana
{
    public interface IAsanaWorkspacesGateway
    {
        Task<AsanaTask> GetTaskAsync(string workspaceGid, string projectGid, string taskName, CancellationToken ct);
    }
}
