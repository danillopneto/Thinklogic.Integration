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
using Thinklogic.Integration.Domain.Azure.PullRequest;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.Gateways.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
        private readonly IAsanaProjectsGateway _asanaProjectsGateway;
        private readonly IAsanaWorkspacesGateway _asanaWorkspacesGateway;
        private readonly DataAppSettings _settings;

        public AsanaFunction(IAsanaProjectsGateway asanaProjectsGateway,
                             IAsanaWorkspacesGateway asanaWorkspacesGateway,
                             DataAppSettings settings)
        {
            _asanaProjectsGateway = asanaProjectsGateway ?? throw new ArgumentNullException(nameof(asanaProjectsGateway));
            _asanaWorkspacesGateway = asanaWorkspacesGateway ?? throw new ArgumentNullException(nameof(asanaWorkspacesGateway));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("UpdateAsanaCommentWithApprovedPR")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            PullRequest data = await req.Content.ReadAsAsync<PullRequest>();

            await NotifyAsana(data);

            return new OkResult();
        }

        private async Task NotifyAsana(PullRequest data)
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
                        var taskRelated = _asanaWorkspacesGateway.GetTaskAsync(_settings.WorkspaceId, projectRelated.Gid, taskName.Trim(), CancellationToken.None);
                        if (taskRelated is not null)
                        {

                        }
                    }
                }
            }
        }
    }
}
