using System;
using System.IO;
using System.Net;   
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using Twilio.TwiML;
using System.Text;

namespace Company.Function
{
    public static class TwiliohttpTriggerCS
    {
        [FunctionName("TwiliohttpTriggerCS")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            
       // var data = await req.Content.ReadAsStringAsync();
         var data = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation(data);
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "), 
                            pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

            // Perform calculations, API lookups, etc. here

            var quest = formValues["Body"];
            log.LogInformation(quest); 

            




            log.LogInformation($"The message is {formValues["Body"]}");

            var response = new MessagingResponse()
                .Message($"This is Shamla teacher , I am here to help you. {formValues["Body"]}");
            var twiml = response.ToString();
            twiml = twiml.Replace("utf-16", "utf-8");

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = twiml,
                StatusCode = 200
            };
        }
    }
}
