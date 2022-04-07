using Thinklogic.Integration.Domain.Dtos.Asana;

namespace Thinklogic.Integration.Interfaces.UseCases.Asana
{
    public interface IInsertCommentAsanaTaskUseCase
    {
        Task<AsanaCommentResultDto> InsertCommentAsync(string workspaceId, string projectName, string taskName, string htmlMessage);
    }
}
