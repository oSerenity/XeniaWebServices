using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace XeniaWebServices
{
    public class RequestBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyMiddleware(RequestDelegate next)
        {
            _next = next;
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
            Console.WriteLine($"Host: {context.Request.Host}");
            Console.WriteLine($"URL: {context.Request.GetDisplayUrl()}");
            Console.WriteLine($"ContentLength: {context.Request.ContentLength}");
            Console.WriteLine($"Protocol: {context.Request.Protocol}");
            Console.WriteLine($"ContentType: {context.Request.ContentType}");
            Console.WriteLine($"Headers: {context.Request.Headers.UserAgent}");
            Console.WriteLine($"method: {context.Request.Method}");
            Console.WriteLine($"path: {context.Request.Path + context.Request.QueryString}");
            Console.WriteLine($"Query: {context.Request.QueryString}");
            Console.WriteLine($"body: {requestBody}");

            // Continue processing the request
            await _next(context);
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
