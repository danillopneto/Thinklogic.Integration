namespace Thinklogic.Integration.Interfaces.UseCases.Asana
{
    public interface IUpdateAsanaTaskCustomFieldUseCase
    {
        Task UpdateTaskCustomFieldAsync(string workspaceId,
                                        string projectName,
                                        string taskName,
                                        string customFieldKey,
                                        string customFieldValue,
                                        CancellationToken ct);
    }
}
