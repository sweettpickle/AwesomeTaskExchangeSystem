using Microsoft.Extensions.Primitives;
using System.Text;

namespace Identity.Web.Utils.Web.Middlewares.Tracing
{
    /// <summary>
    /// Logs request and response into <see cref="ILogger"/>. Logging bodies can be disabled by decorating controller/action with <see cref="DoNotTraceRequestBodyAttribute"/> and <see cref="DoNotTraceResponseBodyAttribute"/>.
    /// </summary>
    internal class HttpTracingMiddleware
    {
        public HttpTracingMiddleware(RequestDelegate next, WebConfig config,
            ILogger<HttpTracingMiddleware> logger)
        {
            _config = config;
            _logger = logger;
            _next = next;
        }

        private readonly RequestDelegate _next;
        private readonly WebConfig _config;
        private readonly ILogger _logger;


        /// <summary>
        /// Handles the request.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public async Task Invoke(HttpContext context)
        {
            if (ShouldNotTrace(context))
            {
                await _next(context);
                return;
            }

            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                { "ActionInfo", context.GetActionInfo() },
                { "TraceId", context.TraceIdentifier },
            });
            await LogRequest(context);

            await _next(context);

            await LogResponse(context);
        }

        #region Private helpers

        //private bool EndpointIsDecoratedWith<TAttribute>(HttpContext context)
        //{
        //    var executingEnpoint = context.GetEndpoint();
        //    return executingEnpoint?.Metadata?.OfType<TAttribute>().Any() ?? false;
        //}

        private async Task LogRequest(HttpContext context)
        {
            var body = context.EndpointIsDecoratedWith<DoNotTraceRequestBodyAttribute>() ? "<skipped>" : await context.ReadRequestAsync();

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, StringValues> header in context.Request.Headers)
                sb.AppendLine($"{header.Key}: {header.Value}");

            _logger.LogDebug(
                 "Http request received:\r\n{Method} {Url}\r\n{Headers}\r\n\r\n{Body}"
                       , context.Request.Method
                       , $"{context.Request.Path}{context.Request.QueryString}"
                       , GetRequestHeaders(context)
                       , body);
        }

        private async Task LogResponse(HttpContext context)
        {
            var body = context.EndpointIsDecoratedWith<DoNotTraceResponseBodyAttribute>() ? "<skipped>" : await context.ReadResponseAsync();

            _logger.LogDebug(
                       "Response sent:\r\n{StatusCode}\r\n{Headers}\r\n\r\n{Body}"
                       , context.Response.StatusCode
                       , GetResponseHeaders(context)
                       , body);
        }

        private bool ShouldNotTrace(HttpContext context) => context.Request.Path.StartsWithSegments("/api") == false;

        private string GetRequestHeaders(HttpContext context)
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, StringValues> header in context.Request.Headers)
                sb.AppendLine($"{header.Key}: {header.Value.ToString()}");
            return sb.ToString();
        }

        private string GetResponseHeaders(HttpContext context)
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
                sb.AppendLine($"{header.Key}: {header.Value.ToString()}");
            return sb.ToString();
        }

        #endregion
    }
}