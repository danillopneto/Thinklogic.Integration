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

        Task UpdateTasksCustomFieldAsync(string workspaceId,
                                         string projectName,
                                         IEnumerable<string> tasksName,
                                         string customFieldKey,
                                         string customFieldValue,
                                         CancellationToken ct);
    }
}