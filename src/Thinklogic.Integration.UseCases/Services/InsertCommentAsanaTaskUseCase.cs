using AutoMapper;
using Thinklogic.Integration.Domain.DataContracts.Requests.Asana;
using Thinklogic.Integration.Domain.Dtos.Asana;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.UseCases.Services
{
    public class InsertCommentAsanaTaskUseCase : IInsertCommentAsanaTaskUseCase
    {
        private readonly IAsanaProjectsGateway _asanaProjectsGateway;
        private readonly IAsanaTasksGateway _asanaTaskGateway;
        private readonly IAsanaWorkspacesGateway _asanaWorkspacesGateway;
        private readonly IMapper _mapper;

        public InsertCommentAsanaTaskUseCase(IAsanaProjectsGateway asanaProjectsGateway,
                                             IAsanaWorkspacesGateway asanaWorkspacesGateway,
                                             IAsanaTasksGateway asanaTaskGateway,
                                             IMapper mapper)
        {
            _asanaProjectsGateway = asanaProjectsGateway ?? throw new ArgumentNullException(nameof(asanaProjectsGateway));
            _asanaWorkspacesGateway = asanaWorkspacesGateway ?? throw new ArgumentNullException(nameof(asanaWorkspacesGateway));
            _asanaTaskGateway = asanaTaskGateway ?? throw new ArgumentNullException(nameof(asanaTaskGateway));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AsanaCommentResultDto> InsertCommentAsync(string workspaceId, string projectName, string taskName, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(workspaceId) ||
                string.IsNullOrWhiteSpace(projectName) ||
                string.IsNullOrWhiteSpace(taskName) ||
                string.IsNullOrWhiteSpace(htmlMessage))
            {
                return default;
            }

            var projects = await _asanaProjectsGateway.GetProjectsAsync(workspaceId, CancellationToken.None);

            var projectRelated = projects.FirstOrDefault(x => x.Name == projectName);
            if (projectRelated is null)
            {
                return default;   
            }

            var taskRelated = await _asanaWorkspacesGateway.GetTaskAsync(workspaceId,
                                                                             projectRelated.Gid,
                                                                             taskName.Trim(),
                                                                             CancellationToken.None);
            if (taskRelated is null)
            {
                return default;
            }

            var result = await _asanaTaskGateway.IncludeCommentAsync(taskRelated.Gid,
                                                                     new AsanaCommentRequest { HtmlText = htmlMessage },
                                                                     CancellationToken.None);
            return _mapper.Map<AsanaCommentResultDto>(result);
        }
    }
}
