namespace Identity.Web.Utils.Web.Middlewares.Rewinding
{
    /// <summary>
    /// Allows to read a response stream multiple times.
    /// </summary>
    internal class ResponseRewindingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebConfig _config;

        public ResponseRewindingMiddleware(RequestDelegate next, WebConfig config)
        {
            _next = next;
            _config = config;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            if (IsApplicable())
                await DoHackyTricks(context);
            else
                await _next(context);

            bool IsApplicable()
            {
                return _config.GetAreaPrefixes().Any(prefix => context.Request.PathBase.StartsWithSegments(prefix));
            }
        }

        private async Task DoHackyTricks(HttpContext context)
        {
            var originaStream = context.Response.Body;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originaStream);
                }
            }
            finally
            {
                context.Response.Body = originaStream;
            }
        }
    }
}