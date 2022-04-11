using Thinklogic.Integration.Domain.DataContracts.Requests.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.UseCases.Services
{
    public class UpdateAsanaTaskCustomFieldUseCase : IUpdateAsanaTaskCustomFieldUseCase
    {
        private readonly IAsanaProjectsGateway _asanaProjectsGateway;
        private readonly IAsanaTasksGateway _asanaTasksGateway;
        private readonly IAsanaWorkspacesGateway _asanaWorkspacesGateway;

        public UpdateAsanaTaskCustomFieldUseCase(IAsanaProjectsGateway asanaProjectsGateway,
                                                 IAsanaWorkspacesGateway asanaWorkspacesGateway,
                                                 IAsanaTasksGateway asanaTasksGateway)
        {
            _asanaProjectsGateway = asanaProjectsGateway ?? throw new ArgumentNullException(nameof(asanaProjectsGateway));
            _asanaWorkspacesGateway = asanaWorkspacesGateway ?? throw new ArgumentNullException(nameof(asanaWorkspacesGateway));
            _asanaTasksGateway = asanaTasksGateway ?? throw new ArgumentNullException(nameof(asanaTasksGateway));
        }

        public async Task UpdateTaskCustomFieldAsync(string workspaceId,
                                                     string projectName,
                                                     string taskName,
                                                     string customFieldKey,
                                                     string customFieldValue,
                                                     CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(workspaceId) ||
                string.IsNullOrWhiteSpace(projectName) ||
                string.IsNullOrWhiteSpace(taskName) ||
                string.IsNullOrWhiteSpace(customFieldKey) ||
                string.IsNullOrWhiteSpace(customFieldValue))
            {
                return;
            }

            var customFieldData = await _asanaWorkspacesGateway.GetCustomFieldAsync(workspaceId, customFieldKey, CancellationToken.None);
            if (customFieldData is null)
            {
                return;
            }

            var customFieldValueData = customFieldData.EnumOptions.FirstOrDefault(x => x.Name == customFieldValue);
            if (customFieldData is null)
            {
                return;
            }

            var projects = await _asanaProjectsGateway.GetProjectsAsync(workspaceId, CancellationToken.None);
            var projectRelated = projects.FirstOrDefault(x => x.Name == projectName);
            if (projectRelated is null)
            {
                return;
            }

            var taskRelated = await _asanaWorkspacesGateway.GetTaskAsync(workspaceId,
                                                                         projectRelated.Gid,
                                                                         taskName.Trim(),
                                                                         CancellationToken.None);
            if (taskRelated is null)
            {
                return;
            }

            await _asanaTasksGateway.UpdateCustomFieldAsync(taskRelated.Gid,
                                                            new AsanaCustomFieldRequest(customFieldData.Gid, customFieldValueData.Gid),
                                                            ct);
        }

        public async Task UpdateTasksCustomFieldAsync(string workspaceId,
                                                      string projectName,
                                                      IEnumerable<string> tasksName,
                                                      string customFieldKey,
                                                      string customFieldValue,
                                                      CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(workspaceId) ||
                string.IsNullOrWhiteSpace(projectName) ||
                tasksName == null ||
                !tasksName.Any() ||
                string.IsNullOrWhiteSpace(customFieldKey) ||
                string.IsNullOrWhiteSpace(customFieldValue))
            {
                return;
            }

            var customFieldData = await _asanaWorkspacesGateway.GetCustomFieldAsync(workspaceId, customFieldKey, CancellationToken.None);
            if (customFieldData is null)
            {
                return;
            }

            var customFieldValueData = customFieldData.EnumOptions.FirstOrDefault(x => x.Name == customFieldValue);
            if (customFieldData is null)
            {
                return;
            }

            var projects = await _asanaProjectsGateway.GetProjectsAsync(workspaceId, CancellationToken.None);
            var projectRelated = projects.FirstOrDefault(x => x.Name == projectName);
            if (projectRelated is null)
            {
                return;
            }

            foreach (var taskName in tasksName)
            {
                var taskRelated = await _asanaWorkspacesGateway.GetTaskAsync(workspaceId,
                                                                             projectRelated.Gid,
                                                                             taskName.Trim(),
                                                                             CancellationToken.None);
                if (taskRelated is null)
                {
                    continue;
                }

                await _asanaTasksGateway.UpdateCustomFieldAsync(taskRelated.Gid,
                                                                new AsanaCustomFieldRequest(customFieldData.Gid, customFieldValueData.Gid),
                                                                ct);
            }
        }
    }
}