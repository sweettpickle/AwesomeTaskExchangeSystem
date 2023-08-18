namespace Identity.Web.Utils.Web.Middlewares.Rewinding
{
    /// <summary>
    /// Grants and ability to read request body multiple times.
    /// </summary>
    internal class RequestRewindingMiddleware
    {
        private readonly WebConfig _config;
        private readonly RequestDelegate _next;

        public RequestRewindingMiddleware(RequestDelegate next, WebConfig config)
        {
            _next = next;
            _config = config;
        }
        /// <summary>
        /// handles the request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (IsApplicable()) context.Request.EnableBuffering();
            await _next(context);

            bool IsApplicable()
            {
                return _config.GetAreaPrefixes().Any(prefix => context.Request.PathBase.StartsWithSegments(prefix));
            }
        }
    }
}