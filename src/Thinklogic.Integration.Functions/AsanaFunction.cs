using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Thinklogic.Integration.Domain.Dtos.Asana;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
        private const string AsanaCommentPath = "asana-comment-path";
        private const string AsanaCustomFieldKey = "asana-custom-field-key";
        private const string AsanaCustomFieldValue = "asana-custom-field-value";
        private const string AsanaTitlePath = "asana-title-path";
        private const string FilterPath = "filter-path";
        private const string FilterValue = "filter-value";

        private readonly IInsertCommentAsanaTaskUseCase _insertCommentAsanaTaskUseCase;
        private readonly DataAppSettings _settings;
        private readonly IUpdateAsanaTaskCustomFieldUseCase _updateAsanaTaskCustomFieldUseCase;

        public AsanaFunction(IInsertCommentAsanaTaskUseCase insertCommentAsanaTaskUseCase,
                             IUpdateAsanaTaskCustomFieldUseCase updateAsanaTaskCustomFieldUseCase,
                             DataAppSettings settings)
        {
            _insertCommentAsanaTaskUseCase = insertCommentAsanaTaskUseCase ?? throw new ArgumentNullException(nameof(insertCommentAsanaTaskUseCase));
            _updateAsanaTaskCustomFieldUseCase = updateAsanaTaskCustomFieldUseCase ?? throw new ArgumentNullException(nameof(updateAsanaTaskCustomFieldUseCase));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("UpdateAsanaTasksCustomField")]
        public async Task<IActionResult> UpdateAsanaTasksCustomField([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var result = ValidateHeader(req,
                                        AsanaTitlePath,
                                        AsanaCustomFieldKey,
                                        AsanaCustomFieldValue);
            if (!result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            var payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
            var parsed = JObject.Parse(payload);

            if (!ShouldProcess(req, parsed))
            {
                return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
            }

            var pathToIdentifyAsanaTask = req.Headers.GetValues(AsanaTitlePath).First();
            var asanaTaskName = parsed.SelectToken(pathToIdentifyAsanaTask).Value<string>();
            if (string.IsNullOrEmpty(asanaTaskName))
            {
                return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
            }

            var projectName = Regex.Match(asanaTaskName, _settings.ProjectNamePattern);
            var tasksName = Regex.Matches(asanaTaskName, _settings.MultiTaskNamePattern);
            var customFieldKey = req.Headers.GetValues(AsanaCustomFieldKey).First();
            var customFieldValue = req.Headers.GetValues(AsanaCustomFieldValue).First();

            await _updateAsanaTaskCustomFieldUseCase.UpdateTasksCustomFieldAsync(_settings.WorkspaceId,
                                                                                 projectName.Value,
                                                                                 tasksName.Select(x => x.Value.Trim()),
                                                                                 customFieldKey,
                                                                                 customFieldValue,
                                                                                 CancellationToken.None);

            log.LogInformation($"Custom Fields Updated made in Workspace {_settings.WorkspaceId}.");

            return new OkObjectResult(Result<string>.Success(default));
        }

        [FunctionName("UpdateAsanaTaskCustomField")]
        public async Task<IActionResult> UpdateAsanaTaskCustomField([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var result = ValidateHeader(req,
                                        AsanaTitlePath,
                                        AsanaCustomFieldKey,
                                        AsanaCustomFieldValue);
            if (!result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            var payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
            var parsed = JObject.Parse(payload);

            if (!ShouldProcess(req, parsed))
            {
                return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
            }

            var pathToIdentifyAsanaTask = req.Headers.GetValues(AsanaTitlePath).First();
            var asanaTaskName = parsed.SelectToken(pathToIdentifyAsanaTask).Value<string>();
            if (string.IsNullOrEmpty(asanaTaskName))
            {
                return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
            }

            var projectName = Regex.Match(asanaTaskName, _settings.ProjectNamePattern);
            var taskName = Regex.Match(asanaTaskName, _settings.TaskNamePattern);
            var customFieldKey = req.Headers.GetValues(AsanaCustomFieldKey).First();
            var customFieldValue = req.Headers.GetValues(AsanaCustomFieldValue).First();

            await _updateAsanaTaskCustomFieldUseCase.UpdateTaskCustomFieldAsync(_settings.WorkspaceId,
                                                                                projectName.Value,
                                                                                taskName.Value,
                                                                                customFieldKey,
                                                                                customFieldValue,
                                                                                CancellationToken.None);

            log.LogInformation($"Custom Field Updated made in Workspace {_settings.WorkspaceId}.");

            return new OkObjectResult(Result<string>.Success(default));
        }

        [FunctionName("UpdateAsanaTaskWithComment")]
        public async Task<IActionResult> UpdateAsanaTaskWithComment([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var result = ValidateHeader(req,
                                        AsanaTitlePath,
                                        AsanaCommentPath);
            if (!result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            var payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
            var parsed = JObject.Parse(payload);

            if (!ShouldProcess(req, parsed))
            {
                return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
            }

            var pathToIdentifyAsanaTask = req.Headers.GetValues(AsanaTitlePath).First();
            var asanaTaskName = parsed.SelectToken(pathToIdentifyAsanaTask).Value<string>();
            if (string.IsNullOrEmpty(asanaTaskName))
            {
                return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
            }

            var projectName = Regex.Match(asanaTaskName, _settings.ProjectNamePattern);
            var taskName = Regex.Match(asanaTaskName, _settings.TaskNamePattern);

            var pathToIdentifyComment = req.Headers.GetValues(AsanaCommentPath).First();
            var taskComment = parsed.SelectToken(pathToIdentifyComment).Value<string>();
            if (string.IsNullOrEmpty(taskComment))
            {
                return ReturnInvalidOperation("Invalid Path to get the Comment Task.");
            }

            AsanaCommentResultDto commentResult = await _insertCommentAsanaTaskUseCase.InsertCommentAsync(_settings.WorkspaceId,
                                                                                                          projectName.Value,
                                                                                                          taskName.Value,
                                                                                                          taskComment);

            log.LogInformation($"Comment made in Workspace {_settings.WorkspaceId}.");

            return new OkObjectResult(Result<AsanaCommentResultDto>.Success(commentResult));
        }

        private static IActionResult ReturnInvalidOperation(string message)
        {
            var result = Result<string>.Invalid(new List<ValidationError> { new ValidationError { ErrorMessage = message } });
            return new OkObjectResult(result);
        }

        private static bool ShouldProcess(HttpRequestMessage req, JObject data)
        {
            if (!req.Headers.Contains(FilterPath))
            {
                return true;
            }

            string pathToFilterData = req.Headers.GetValues(FilterPath).FirstOrDefault();
            string filterValue = req.Headers.GetValues(FilterValue).FirstOrDefault();
            return data.SelectToken(pathToFilterData).Value<string>().Contains(filterValue);
        }

        private static Result<bool> ValidateHeader(HttpRequestMessage req, params string[] headerKeys)
        {
            var validations = new List<ValidationError>();
            foreach (var headerKey in headerKeys)
            {
                if (!req.Headers.Contains(headerKey))
                {
                    validations.Add(new ValidationError { ErrorMessage = $"{headerKey} was not found inside the header.", Identifier = headerKey });
                }
            }

            return validations.Any() ? Result<bool>.Invalid(validations) : Result<bool>.Success(true);
        }
    }
}