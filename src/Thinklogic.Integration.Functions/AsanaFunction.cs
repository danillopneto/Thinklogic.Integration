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
using Thinklogic.Integration.Functions.Models;
using Thinklogic.Integration.Infrastructure.Configurations;
using Thinklogic.Integration.Interfaces.UseCases.Asana;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public class AsanaFunction
    {
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

        [FunctionName("UpdateAsanaTaskCustomField")]
        public async Task<IActionResult> UpdateAsanaTaskCustomField([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                FunctionParameters parameters = new FunctionParameters(req);
                Result<bool> result = ValidateParameters(parameters.AsanaProjectPath,
                                                         parameters.AsanaTaskPath,
                                                         parameters.AsanaCustomFieldKey,
                                                         parameters.AsanaCustomFieldValue);
                if (!result.IsSuccess)
                {
                    return new OkObjectResult(result);
                }

                string payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
                JObject parsed = JObject.Parse(payload);

                if (!ShouldProcess(parameters, parsed))
                {
                    return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
                }

                string asanaProjectName = parsed.SelectToken(parameters.AsanaProjectPath).Value<string>();
                if (string.IsNullOrEmpty(asanaProjectName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Project.");
                }

                string asanaTaskName = parsed.SelectToken(parameters.AsanaTaskPath).Value<string>();
                if (string.IsNullOrEmpty(asanaTaskName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
                }

                string projectName = Regex.Match(asanaProjectName, _settings.ProjectNamePattern).Value;
                string taskName = Regex.Match(asanaTaskName, _settings.TaskNamePattern).Value;
                string asanaCustomFieldValue = GetAsanaCustomFieldValue(parameters, parsed);

                await _updateAsanaTaskCustomFieldUseCase.UpdateTaskCustomFieldAsync(_settings.WorkspaceId,
                                                                                    projectName,
                                                                                    taskName,
                                                                                    parameters.AsanaCustomFieldKey,
                                                                                    asanaCustomFieldValue,
                                                                                    CancellationToken.None);

                log.LogInformation($"Custom Field Updated made in Workspace {_settings.WorkspaceId}.");

                return new OkObjectResult(Result<string>.Success($"Custom Field Updated made in Workspace {_settings.WorkspaceId}.\nProject: {projectName}\nTask: {taskName}"));
            }
            catch (Exception ex)
            {
                log.LogError("Error updating asana custom field task.", ex);
                return ReturnInvalidOperation(ex.Message);
            }
        }

        [FunctionName("UpdateAsanaTasksCustomField")]
        public async Task<IActionResult> UpdateAsanaTasksCustomField([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                FunctionParameters parameters = new FunctionParameters(req);
                Result<bool> result = ValidateParameters(parameters.AsanaProjectPath,
                                                         parameters.AsanaTaskPath,
                                                         parameters.AsanaCustomFieldKey,
                                                         parameters.AsanaCustomFieldValue);
                if (!result.IsSuccess)
                {
                    return new OkObjectResult(result);
                }

                string payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
                JObject parsed = JObject.Parse(payload);

                if (!ShouldProcess(parameters, parsed))
                {
                    return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
                }

                string asanaProjectName = parsed.SelectToken(parameters.AsanaProjectPath).Value<string>();
                if (string.IsNullOrEmpty(asanaProjectName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Project.");
                }

                string asanaTaskName = parsed.SelectToken(parameters.AsanaTaskPath).Value<string>();
                if (string.IsNullOrEmpty(asanaTaskName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
                }

                string projectName = Regex.Match(asanaProjectName, _settings.ProjectNamePattern).Value;
                IEnumerable<string> tasksName = Regex.Matches(asanaTaskName, _settings.MultiTaskNamePattern).Select(x => x.Value.Trim());
                string asanaCustomFieldValue = GetAsanaCustomFieldValue(parameters, parsed);

                await _updateAsanaTaskCustomFieldUseCase.UpdateTasksCustomFieldAsync(_settings.WorkspaceId,
                                                                                     projectName,
                                                                                     tasksName,
                                                                                     parameters.AsanaCustomFieldKey,
                                                                                     asanaCustomFieldValue,
                                                                                     CancellationToken.None);

                log.LogInformation($"Custom Fields Updated made in Workspace {_settings.WorkspaceId}.");

                return new OkObjectResult(Result<string>.Success(default));
            }
            catch (Exception ex)
            {
                log.LogError("Error updating asana custom field task.", ex);
                return ReturnInvalidOperation(ex.Message);
            }
        }

        [FunctionName("UpdateAsanaTaskWithComment")]
        public async Task<IActionResult> UpdateAsanaTaskWithComment([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
                                                                    ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                FunctionParameters parameters = new FunctionParameters(req);
                Result<bool> result = ValidateParameters(parameters.AsanaProjectPath,
                                                         parameters.AsanaTaskPath,
                                                         parameters.AsanaCommentPath);
                if (!result.IsSuccess)
                {
                    return new OkObjectResult(result);
                }

                string payload = HttpUtility.HtmlDecode(await req.Content.ReadAsStringAsync());
                JObject parsed = JObject.Parse(payload);

                if (!ShouldProcess(parameters, parsed))
                {
                    return new OkObjectResult(Result<string>.Success(default, "It wasn't necessary to process."));
                }

                string asanaProjectName = parsed.SelectToken(parameters.AsanaProjectPath).Value<string>();
                if (string.IsNullOrEmpty(asanaProjectName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Project.");
                }

                string asanaTaskName = parsed.SelectToken(parameters.AsanaTaskPath).Value<string>();
                if (string.IsNullOrEmpty(asanaTaskName))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Asana Task.");
                }

                string projectName = Regex.Match(asanaProjectName, _settings.ProjectNamePattern).Value;
                string taskName = Regex.Match(asanaTaskName, _settings.TaskNamePattern).Value;

                string taskComment = parsed.SelectToken(parameters.AsanaCommentPath).Value<string>();
                if (string.IsNullOrEmpty(taskComment))
                {
                    return ReturnInvalidOperation("Invalid Path to get the Comment Task.");
                }

                AsanaCommentResultDto commentResult = await _insertCommentAsanaTaskUseCase.InsertCommentAsync(_settings.WorkspaceId,
                                                                                                              projectName,
                                                                                                              taskName,
                                                                                                              taskComment);

                log.LogInformation($"Comment made in Workspace {_settings.WorkspaceId}.");

                return new OkObjectResult(Result<AsanaCommentResultDto>.Success(commentResult));
            }
            catch (Exception ex)
            {
                log.LogError("Error updating asana custom field task.", ex);
                return ReturnInvalidOperation(ex.Message);
            }
        }

        private static string GetAsanaCustomFieldValue(FunctionParameters parameters, JObject parsed)
        {
            try
            {
                return parsed.SelectToken(parameters.AsanaCustomFieldValue)?.Value<string>() ?? parameters.AsanaCustomFieldValue;
            }
            catch
            {
                return parameters.AsanaCustomFieldValue;
            }
        }

        private static IActionResult ReturnInvalidOperation(string message)
        {
            var result = Result<string>.Invalid(new List<ValidationError> { new ValidationError { ErrorMessage = message } });
            return new OkObjectResult(result);
        }

        private static bool ShouldProcess(FunctionParameters parameters, JObject data)
        {
            return string.IsNullOrWhiteSpace(parameters.FilterPath) ||
                   data.SelectToken(parameters.FilterPath).Value<string>().Contains(parameters.FilterValue);
        }

        private static Result<bool> ValidateParameters(params string[] headerKeys)
        {
            var validations = new List<ValidationError>();
            foreach (var headerKey in headerKeys)
            {
                if (string.IsNullOrWhiteSpace(headerKey))
                {
                    validations.Add(new ValidationError { ErrorMessage = $"Some data were not found inside the header." });
                }
            }

            return validations.Any() ? Result<bool>.Invalid(validations) : Result<bool>.Success(true);
        }
    }
}