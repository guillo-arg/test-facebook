using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace test_facebook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacebookController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FacebookController> _logger;

        public FacebookController(ILogger<FacebookController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> get() 
        {
            var token = Request.Query["hub.verify_token"];
            string response = Request.Query["hub.challenge"];
            _logger.LogInformation($"QueryString: {Request.QueryString.Value}");
            _logger.LogInformation($"Content-type: {Request.ContentType}");
            _logger.LogInformation($"X-Forwarded-For: {Request.Headers["X-Forwarded-For"]}");

            return Ok(response);

            
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestDTO requestDTO)
        {
            _logger.LogInformation("Entro a post");
            return Ok();
        }


    }
}
