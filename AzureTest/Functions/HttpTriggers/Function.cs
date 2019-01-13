using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureTest.Functions.Models;
using System.Security.Cryptography;
using System.Text;

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
                using (var sha256 = SHA256.Create())
                {
                    var hash = sha256.ComputeHash(executionContext.InvocationId.ToByteArray());
                    return new OkObjectResult(ToHex(hash));
                }
            }
            catch(Exception exception)
            {
                log?.LogError("[{0}:{1}] {2}", executionContext.InvocationId, executionContext.FunctionName, exception.Message);
                return new BadRequestObjectResult(exception.Message);
            }
        }

        private static string ToHex(byte[] bytes, bool upperCase = false)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }
    }
}
