using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureTest.Functions.Models;

namespace AzureTest
{
    public static class Function
    {
        [FunctionName("Function")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ExecutionContext executionContext,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(request.Body).ReadToEnd();
                FunctionModel httpTriggerModel = JsonConvert.DeserializeObject<FunctionModel>(requestBody);

                return string.IsNullOrWhiteSpace(httpTriggerModel.Name)
                    ? new BadRequestObjectResult("Please pass a 'Name' in the request body")
                    : (ActionResult)new OkObjectResult($"Hello, {httpTriggerModel.Name}");
            }
            catch(Exception exception)
            {
                log?.LogError("[{0}:{1}] {2}", executionContext.InvocationId, executionContext.FunctionName, exception.Message);
                return new OkObjectResult(exception.Message);
            }
        }
    }
}
