using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;

namespace Thinklogic.Integration.Interfaces.Gateways.Asana
{
    public interface IAsanaProjectsGateway
    {
        Task<IEnumerable<AsanaProjectResponse>> GetProjectsAsync(string workspaceGid, CancellationToken ct);
    }
}
