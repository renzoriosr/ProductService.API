using System.Diagnostics;

namespace ProductService.Api.Middleware
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseTimeMiddleware> _logger;

        public ResponseTimeMiddleware(RequestDelegate next, ILogger<ResponseTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();

            string endpoint = context.Request.Path.Value;
            var requestMethod = context.Request.Method;

            watch.Start();
            context.Response.OnStarting(() => {
                watch.Stop();
                var requestExecutionTime = watch.ElapsedMilliseconds;

                string message = $"{requestMethod} {endpoint} - duration: {requestExecutionTime} ms";
                
                _logger.LogWarning(message);
                
                return Task.CompletedTask;
            });

            return this._next(context);
        }
    }
}
