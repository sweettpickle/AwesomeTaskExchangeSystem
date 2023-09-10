
namespace Accounting.Web.Utils
{
    /// <summary>
    /// Contains Accounting.Web API related settings.
    /// </summary>
    public class WebConfig
    {
        public int Port { get; set; }
        /// <summary>
        /// Sets the language in which the api will respond if the Accept-Language header is missing in the request.
        /// </summary>
        public string DefaultLocale { get; set; }
        /// <summary>
        /// Contains various settings for the private part of the API used by the internal services of the GSS ecosystem
        /// </summary>
        public ApiAreaConfig InternalApiConfig { get; set; }

        /// <summary>
        /// Returns a list of all area`s prefixes
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetAreaPrefixes()
        {
            return GetKnownAreas().Select(a => a.RoutePrefix);
        }

        internal IEnumerable<ApiAreaConfig> GetKnownAreas()
        {
            // register other areas here
            yield return InternalApiConfig;
        }
    }

    public class ApiAreaConfig
    {
        /// <summary>
        /// Route starting prefix for an area.
        /// </summary>
        public string RoutePrefix { get; set; }
        /// <summary>
        /// Swagger`s doc name.
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// Relative path to Swagger endpoint.
        /// </summary>
        public string SwaggerEndpointUrl { get; set; }
        /// <summary>
        /// API`s title for Swagger documentation.
        /// </summary>
        public string SwaggerTitle { get; set; }
        /// <summary>
        /// Base controller`s type short name.
        /// </summary>
        public string ControllerBaseClassName { get; set; }
    }
}