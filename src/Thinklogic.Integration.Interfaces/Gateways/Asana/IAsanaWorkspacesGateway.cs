using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;

namespace Thinklogic.Integration.Interfaces.Gateways.Asana
{
    public interface IAsanaWorkspacesGateway
    {
        Task<AsanaTaskResponse> GetTaskAsync(string workspaceGid, string projectGid, string taskName, CancellationToken ct);
    }
}
