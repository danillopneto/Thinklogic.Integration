using Thinklogic.Integration.Domain.DataContracts.Requests.Asana;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;

namespace Thinklogic.Integration.Interfaces.Gateways.Asana
{
    public interface IAsanaTasksGateway
    {
        Task<AsanaCommentResponse> IncludeCommentAsync(string taskGid,
                                                       AsanaCommentRequest comment,
                                                       CancellationToken ct);

        Task UpdateCustomFieldAsync(string taskGid,
                                    AsanaCustomFieldRequest customField,
                                    CancellationToken ct);
    }
}
