using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using AzureTest.Services;
using AzureTest.Functions.Models;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace AzureTest
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ExecutionContext executionContext,
            ILogger log)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hash;

                    var requestBody = await (new StreamReader(request.Body)).ReadToEndAsync().ConfigureAwait(false);
                    FunctionModel httpTriggerModel = JsonConvert.DeserializeObject<FunctionModel>(requestBody);

                    if(httpTriggerModel?.Name == null || string.IsNullOrWhiteSpace(httpTriggerModel.Name))
                    {
                        hash = sha256.ComputeHash(executionContext.InvocationId.ToByteArray());
                    }
                    else
                    {
                        hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(httpTriggerModel.Name));
                    }

                    return new OkObjectResult(HexService.ToHex(hash));
                }
            }
            catch(Exception exception)
            {
                log?.LogError("[{0}:{1}] {2}", executionContext.InvocationId, executionContext.FunctionName, exception.Message);
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
