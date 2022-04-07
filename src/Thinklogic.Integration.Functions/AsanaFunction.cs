using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Thinklogic.Integration.Domain.Dtos.Asana;
using Thinklogic.Integration.Domain.Dtos.Azure.PullRequest;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
        private readonly IInsertCommentAsanaTaskUseCase _insertCommentAsanaTaskUseCase;
        private readonly DataAppSettings _settings;

        public AsanaFunction(IInsertCommentAsanaTaskUseCase insertCommentAsanaTaskUseCase,
                             DataAppSettings settings)
        {
            _insertCommentAsanaTaskUseCase = insertCommentAsanaTaskUseCase ?? throw new ArgumentNullException(nameof(insertCommentAsanaTaskUseCase));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("UpdateAsanaCommentWithPRUpdate")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            PullRequestDto data = await req.Content.ReadAsAsync<PullRequestDto>();

            var projectName = Regex.Match(data.Resource.Title, _settings.ProjectNamePattern);
            var taskName = Regex.Replace(data.Resource.Title, _settings.TaskNameReplacePattern, string.Empty);

            AsanaCommentResultDto result = await _insertCommentAsanaTaskUseCase.InsertCommentAsync(_settings.WorkspaceId, projectName.Value, taskName, data.Message.Html);
            log.LogInformation($"Comment mady in Workspace {_settings.WorkspaceId}");
            return new OkObjectResult(result);
        }
    }
}
