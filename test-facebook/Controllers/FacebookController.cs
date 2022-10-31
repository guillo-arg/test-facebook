using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace test_facebook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacebookWebtokenController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FacebookWebtokenController> _logger;
        private readonly string _token;

        public FacebookWebtokenController(ILogger<FacebookWebtokenController> logger)
        {
            _logger = logger;
            _token = "EABIwcGfg7ZCIBAFmVwg3MS09LG1kWckuotVrMDGEYSFuhpt3Av6nR05mgWF2mQltFOvPoZCgZARPvr26nOXuKziwPJyiXGiWG5IZCQRrPNOwCwkI11li00DZA7IyR1KZCMJlsXqKfd4i5nZCPYxtL3Qk9gsPyH6Wn2IVLWHQ8IK1kwEAyIDSz8M";
        }

        [HttpGet]
        public async Task<IActionResult> get() 
        {
            var token = Request.Query["hub.verify_token"];
            string response = Request.Query["hub.challenge"];
            _logger.LogInformation($"QueryString: {Request.QueryString.Value}");
            _logger.LogInformation($"Content-type: {Request.ContentType}");
            _logger.LogInformation($"X-Forwarded-For: {Request.Headers["X-Forwarded-For"]}");

            if (_token == token)
            {
                return Ok(response);
            }
            else
            {
                return Forbid();
            }



            
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestDTO requestDTO)
        {
            _logger.LogInformation("Entro a post");

            string json = JsonConvert.SerializeObject(requestDTO);

            string xHubSignatureSha1 = Request.Headers["X-Hub-Signature"];
            string xHubSignatureSha256 = Request.Headers["X-Hub-Signature-256"];
            _logger.LogInformation($"X-Hub-Signature: {xHubSignatureSha1}");
            _logger.LogInformation($"X-Hub-Signature-256: {xHubSignatureSha256}");
            _logger.LogInformation($"json: {json}");


            if (string.IsNullOrEmpty(xHubSignatureSha256))
            {
                return Forbid();
            }




            return Ok();
        }


    }
}
