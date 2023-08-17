using Identity.Application.Utils.Common;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text;

namespace Identity.Web.Utils.Web
{
    /// <summary>
    /// Helper methods for working with HttpContext.
    /// </summary>
    public static class HttpContextHelper
    {
        private const string CallerIdentityPropertyKey = "CallerIdentity";

        /// <summary>
        /// Reads the raw request contents.
        /// </summary>
        public static async Task<string> ReadRequestAsync(this HttpContext context)
        {
            var request = context.Request;

            request.Body.Rewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var text = Encoding.UTF8.GetString(buffer);

            request.Body.Rewind();

            return text;
        }

        /// <summary>
        /// Reads the raw response contents.
        /// </summary>
        public static async Task<string> ReadResponseAsync(this HttpContext context)
        {
            var response = context.Response;

            response.Body.Rewind();
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Rewind();

            return text;
        }

        public static bool EndpointIsDecoratedWith<TAttribute>(this HttpContext context)
        {
            var executingEnpoint = context.GetEndpoint();
            return executingEnpoint?.Metadata?.OfType<TAttribute>().Any() ?? false;
        }

        public static CallerIdentity GetCallerIdentity(this HttpContext context)
        {
            return (CallerIdentity)context.Items[CallerIdentityPropertyKey];
        }

        public static void SetCallerIdentity(this HttpContext context, CallerIdentity callContext)
        {
            context.Items[CallerIdentityPropertyKey] = callContext;
        }

        /// <summary>
        /// Returns action`s information in form 'ControllerName.ActionName'.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetActionInfo(this HttpContext context)
        {
            var descriptor = context.GetEndpoint()?.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();

            return descriptor == null
                ? String.Empty
                : $"{descriptor.ControllerName}.{descriptor.ActionName}";
        }

        /// <summary>
        /// Rewinds the stream to the beginning.
        /// </summary>
        private static void Rewind(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}