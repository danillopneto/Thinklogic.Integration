using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Thinklogic.Integration.Domain.Dtos.Asana;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
        private const string AsanaTitlePath = "asana-title-path";
        private const string AsanaCommentPath = "asana-comment-path";

        private readonly IInsertCommentAsanaTaskUseCase _insertCommentAsanaTaskUseCase;
        private readonly DataAppSettings _settings;

        public AsanaFunction(IInsertCommentAsanaTaskUseCase insertCommentAsanaTaskUseCase,
                             DataAppSettings settings)
        {
            _insertCommentAsanaTaskUseCase = insertCommentAsanaTaskUseCase ?? throw new ArgumentNullException(nameof(insertCommentAsanaTaskUseCase));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("UpdateAsanaTaskWithComment")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                             ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (!req.Headers.Contains(AsanaTitlePath))
            {
                throw new NullReferenceException($"{AsanaTitlePath} was not found inside the header.");
            }

            if (!req.Headers.Contains(AsanaCommentPath))
            {
                throw new NullReferenceException($"{AsanaCommentPath} was not found inside the header.");
            }

            var payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
            var parsed = JObject.Parse(payload);

            var pathToIdentifyAsanaTask = req.Headers.GetValues(AsanaTitlePath).First();
            var asanaTaskName = parsed.SelectToken(pathToIdentifyAsanaTask).Value<string>();
            if (string.IsNullOrEmpty(asanaTaskName))
            {
                throw new InvalidOperationException("Invalid Path to get the Asana Task.");
            }

            var projectName = Regex.Match(asanaTaskName, _settings.ProjectNamePattern);
            var taskName = Regex.Replace(asanaTaskName, _settings.TaskNameReplacePattern, string.Empty);

            var pathToIdentifyComment = req.Headers.GetValues(AsanaCommentPath).First();
            var taskComment = parsed.SelectToken(pathToIdentifyComment).Value<string>();
            if (string.IsNullOrEmpty(taskComment))
            {
                throw new InvalidOperationException("Invalid Path to get the Comment Task.");
            }

            AsanaCommentResultDto result = await _insertCommentAsanaTaskUseCase.InsertCommentAsync(_settings.WorkspaceId,
                                                                                                   projectName.Value,
                                                                                                   taskName,
                                                                                                   taskComment);

            log.LogInformation($"Comment made in Workspace {_settings.WorkspaceId}.");

            return new OkObjectResult(result);
        }
    }
}
