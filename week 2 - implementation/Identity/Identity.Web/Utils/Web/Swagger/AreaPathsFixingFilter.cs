using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Identity.Web.Utils.Web.Swagger
{
    /// <summary>
    /// Corrects a swagger documents by adding a route prefix to all paths.
    /// </summary>
    internal class AreaPathsFixingFilter : IDocumentFilter
    {
        private readonly WebConfig _config;

        public AreaPathsFixingFilter(WebConfig config)
        {
            _config = config;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (TryFindAreaBy(context.DocumentName, out var area) == false) return;

            swaggerDoc.Paths = BuildFixedPathsFrom(swaggerDoc.Paths, area);
        }

        #region private members

        private KeyValuePair<string, OpenApiPathItem> BuildPathWithRoutePrefix(KeyValuePair<string, OpenApiPathItem> source,
            string routePrefix)
        {
            if (source.Key.StartsWith(routePrefix)) return source;
            var newKey = $"{routePrefix}{source.Key}";
            return new KeyValuePair<string, OpenApiPathItem>(newKey, source.Value);
        }

        private bool TryFindAreaBy(string name, out ApiAreaConfig cfg)
        {
            cfg = _config.GetKnownAreas().FirstOrDefault(a => a.DocumentName == name);

            return cfg != null;
        }

        private OpenApiPaths BuildFixedPathsFrom(OpenApiPaths source, ApiAreaConfig area)
        {
            var routePrefix = area.RoutePrefix.StartsWith('/')
                ? area.RoutePrefix
                : $"/{area.RoutePrefix}";

            var fixedPaths = new OpenApiPaths();

            foreach (var path in source)
            {
                var (key, value) = BuildPathWithRoutePrefix(path, routePrefix);
                fixedPaths.Add(key, value);
            }

            return fixedPaths;
        }

        #endregion

    }
}