using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AzureTest.Functions.Models;

namespace AzureTest
{
    public static class HttpTrigger
    {
        [FunctionName("HttpTrigger")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request)
        { 
            string requestBody = new StreamReader(request.Body).ReadToEnd();
            HttpTriggerModel httpTriggerModel = JsonConvert.DeserializeObject<HttpTriggerModel>(requestBody);

            return string.IsNullOrWhiteSpace(httpTriggerModel.Name) 
                ? new BadRequestObjectResult("Please pass a 'Name' in the request body")
                : (ActionResult)new OkObjectResult($"Hello, {httpTriggerModel.Name}");
        }
    }
}
