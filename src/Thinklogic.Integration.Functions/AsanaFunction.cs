using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.Gateways.Asana;
using Thinklogic.Integration.Domain.Dtos.Azure.PullRequest;
using Thinklogic.Integration.Domain.DataContracts.Requests.Asana;
using Thinklogic.Integration.Domain.DataContracts.Responses.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
        private readonly IAsanaProjectsGateway _asanaProjectsGateway;
        private readonly IAsanaTasksGateway _asanaTaskGateway;
        private readonly IAsanaWorkspacesGateway _asanaWorkspacesGateway;
        private readonly DataAppSettings _settings;

        public AsanaFunction(IAsanaProjectsGateway asanaProjectsGateway,
                             IAsanaWorkspacesGateway asanaWorkspacesGateway,
                             IAsanaTasksGateway asanaTaskGateway,
                             DataAppSettings settings)
        {
            _asanaProjectsGateway = asanaProjectsGateway ?? throw new ArgumentNullException(nameof(asanaProjectsGateway));
            _asanaWorkspacesGateway = asanaWorkspacesGateway ?? throw new ArgumentNullException(nameof(asanaWorkspacesGateway));
            _asanaTaskGateway = asanaTaskGateway ?? throw new ArgumentNullException(nameof(asanaTaskGateway));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("UpdateAsanaCommentWithApprovedPR")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            PullRequestDto data = await req.Content.ReadAsAsync<PullRequestDto>();

            return new OkObjectResult(await NotifyAsana(data));
        }

        private async Task<AsanaCommentResponse> NotifyAsana(PullRequestDto data)
        {
            var projects = await _asanaProjectsGateway.GetProjectsAsync(_settings.WorkspaceId, CancellationToken.None);

            var projectName = Regex.Match(data.Resource.Title, _settings.ProjectNamePattern);
            if (projectName.Success)
            {
                var projectRelated = projects.FirstOrDefault(x => x.Name == projectName.Value);
                if (projectRelated is not null)
                {
                    var taskName = Regex.Replace(data.Resource.Title, _settings.TaskNameReplacePattern, string.Empty);
                    if (!string.IsNullOrEmpty(taskName))
                    {
                        var taskRelated = await _asanaWorkspacesGateway.GetTaskAsync(_settings.WorkspaceId, projectRelated.Gid, taskName.Trim(), CancellationToken.None);
                        if (taskRelated is not null)
                        {
                            return await _asanaTaskGateway.IncludeCommentAsync(taskRelated.Gid, new AsanaCommentRequest { HtmlText = data.Message.Html });
                        }
                    }
                }
            }

            return default;
        }
    }
}
