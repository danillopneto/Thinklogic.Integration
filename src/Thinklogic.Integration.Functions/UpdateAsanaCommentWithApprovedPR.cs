using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Thinklogic.Integration.Domain.Azure.PullRequest;

namespace Thinklogic.Integration.Functions.WebHooks
{
    public static class UpdateAsanaCommentWithApprovedPR
    {
        [FunctionName("UpdateAsanaCommentWithApprovedPR")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PullRequest data = JsonConvert.DeserializeObject<PullRequest>(requestBody);

            return new OkObjectResult(data);
        }
    }
}
