
namespace Identity.Web.Utils
{
    /// <summary>
    /// Contains Identity.Web API related settings.
    /// </summary>
    public class WebConfig
    {
        public int Port { get; set; }
        
        public ApiAreaConfig ApiConfig { get; set; }

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
            yield return ApiConfig;
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
    }
}