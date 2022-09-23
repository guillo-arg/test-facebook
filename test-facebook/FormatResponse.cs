using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_facebook
{
    public class FormatResponse
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public FormatResponse(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _logger = logger.CreateLogger<FormatResponse>();
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                StringValues val;

                String FinalResponse = string.Empty;
                using (var responseBody = new MemoryStream())
                {
                    var body = context.Response.Body;

                    using (var updatedBody = new MemoryStream())
                    {
                        context.Response.Body = updatedBody;
                        var verbo = context.Request.Method;


                        var request = context.Request;
                        var stream = request.Body;// currently holds the original stream                    
                        var originalReader = new StreamReader(stream);
                        var originalContent = await originalReader.ReadToEndAsync();


                        _logger.LogInformation($"Contenido: {originalContent}");
                        _logger.LogInformation($"método: {context.Request.Method}");
                        _logger.LogInformation($"path: {context.Request.Path}");
                        _logger.LogInformation($"content-type:{context.Request.ContentType}");
                        _logger.LogInformation($"queryString: {context.Request.QueryString}");

                        var requestData = Encoding.UTF8.GetBytes(originalContent);
                        stream = new MemoryStream(requestData);
                        request.Body = stream;


                        await _next(context);

                        context.Response.Body = body;

                        updatedBody.Seek(0, SeekOrigin.Begin);
                        var newContent = new StreamReader(updatedBody).ReadToEnd();


                       
                        await context.Response.WriteAsync(FinalResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Middleware ResponseFormat-> Unexpected Exception:  Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync("Error interno");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FormatResponseExtensions
    {
        public static IApplicationBuilder UseFormatResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FormatResponse>();
        }
    }
}
