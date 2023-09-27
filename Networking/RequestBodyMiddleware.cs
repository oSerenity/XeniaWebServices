using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace XeniaWebServices.Networking
{
    public class RequestBodyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _enableDebugLog = true;
        private readonly bool _writeToFile = true;
        // Inject the ILogger<YourControllerName> into your controller or service
        private readonly ILogger<RequestBodyMiddleware> _logger;
        public RequestBodyMiddleware(ILogger<RequestBodyMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering(); // Enable buffering to read the request body multiple times

            var request = context.Request;
            string requestBody;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                // Store the request body in a property accessible to controllers
                context.Items["RequestBody"] = requestBody;
                // Reset the stream position so that it can be read again
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            if (_enableDebugLog)
            {
                DebugLog(context, requestBody);
            }

            WriteLog(context, requestBody);


            // Continue processing the request
            await _next(context);
        }

        private void DebugLog(HttpContext context, string requestBody)
        {
            Console.WriteLine("---------- Request Details ----------");
            Console.WriteLine($"Host: {context.Request.Host}");
            Console.WriteLine($"Method: {context.Request.Method}");
            Console.WriteLine($"Protocol: {context.Request.Protocol}");
            Console.WriteLine($"URL: {context.Request.GetDisplayUrl()}");
            Console.WriteLine($"Path: {context.Request.Path + context.Request.QueryString}");

            foreach (var header in context.Request.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            if (context.Request.ContentLength != null)
            {
                Console.WriteLine($"ContentLength: {context.Request.ContentLength}");
            }

            Console.WriteLine($"ContentType: {context.Request.ContentType}");

            if (!string.IsNullOrEmpty(requestBody))
            {
                Console.WriteLine("---------- Request Body ----------");
                Console.WriteLine(requestBody);
            }

            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine();
        }

        private void WriteLog(HttpContext context, string requestBody)
        {
            if (_writeToFile)
            {
                try
                {
                    using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.log"), true))
                    {
                        writer.WriteLine("---------- Request Details ----------");
                        writer.WriteLine($"Host: {context.Request.Host}");
                        writer.WriteLine($"Method: {context.Request.Method}");
                        writer.WriteLine($"Protocol: {context.Request.Protocol}");
                        writer.WriteLine($"URL: {context.Request.GetDisplayUrl()}");
                        writer.WriteLine($"Path: {context.Request.Path + context.Request.QueryString}");

                        foreach (var header in context.Request.Headers)
                        {
                            writer.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }

                        if (context.Request.ContentLength != null)
                        {
                            writer.WriteLine($"ContentLength: {context.Request.ContentLength}");
                        }

                        writer.WriteLine($"ContentType: {context.Request.ContentType}");

                        if (!string.IsNullOrEmpty(requestBody))
                        {
                            writer.WriteLine("---------- Request Body ----------");
                            writer.WriteLine(requestBody);
                        }

                        writer.WriteLine("--------------------------------------------");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }

    }

    public static class RequestBodyMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestBodyMiddleware(this IApplicationBuilder app, Func<HttpContext, Func<Task>, Task> middleware)
        {
            return app.UseMiddleware<RequestBodyMiddleware>();
        }
    }
}
