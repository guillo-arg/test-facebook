using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
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

            var stream = Request.Body;                  
            var originalReader = new StreamReader(stream);
            var body = await originalReader.ReadToEndAsync();

            string json = JsonConvert.SerializeObject(body);

            string xHubSignatureSha1 = Request.Headers["X-Hub-Signature"];
            string xHubSignatureSha256 = Request.Headers["X-Hub-Signature-256"];
            _logger.LogInformation($"X-Hub-Signature: {xHubSignatureSha1}");
            _logger.LogInformation($"X-Hub-Signature-256: {xHubSignatureSha256}");
            _logger.LogInformation($"json: {json}");


            if (string.IsNullOrEmpty(xHubSignatureSha256))
            {
                return Forbid();
            }

            string sign = CalculateSignature(_token, json);

            _logger.LogInformation($"Firma App: {sign}");




            return Ok();
        }

        /// <summary>
        /// The HTTP request will contain an X-Hub-Signature header which contains the SHA1 signature of the request payload,
        /// using the app secret as the key, and prefixed with sha1=.
        /// Your callback endpoint can verify this signature to validate the integrity and origin of the payload
        /// </summary>
        /// <param name="appSecret">facebook app secret</param>
        /// <param name="payload">body of webhook post request</param>
        /// <returns>calculated signature</returns>
        public static string CalculateSignature(string appSecret, string payload)
        {
            /*
             Please note that the calculation is made on the escaped unicode version of the payload, with lower case hex digits.
             If you just calculate against the decoded bytes, you will end up with a different signature.
             For example, the string äöå should be escaped to \u00e4\u00f6\u00e5.
             */
            payload = EncodeNonAsciiCharacters(payload);

            byte[] secretKey = Encoding.UTF8.GetBytes(appSecret);
            HMACSHA1 hmac = new HMACSHA1(secretKey);
            hmac.Initialize();
            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            byte[] rawHmac = hmac.ComputeHash(bytes);

            return ByteArrayToString(rawHmac).ToLower();
        }

        private static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }


    }
}
