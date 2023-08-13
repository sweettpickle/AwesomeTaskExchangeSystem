using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using TaskManager.Web.Utils.Web.Middlewares.Rewinding;
using TaskManager.Web.Utils.Web.Middlewares.Tracing;
using TaskManager.Web.Utils.Web.Swagger;

namespace TaskManager.Web.Utils
{
    /// <summary>
    /// Configures hosting of all web api`s and UI`s provided by application.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Configures hosting of all web api`s and UI`s provided by application.
        /// </summary>
        /// <param name="webHostBuilder"></param>
        /// <returns></returns>
        public static IWebHostBuilder AddWeb(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder
                .UseKestrel((context, opts) =>
                {
                    var config = context.GetConfig();
                    opts.ListenAnyIP(config.Port);
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        var executingAssembly = Assembly.GetExecutingAssembly();
                        var config = context.GetConfig();

                        services
                            .AddSingleton(config)
                            .AddSingleton<AreaPathsFixingFilter>()
                            .AddMediatR(cfg =>
                            {
                                cfg.RegisterServicesFromAssembly(executingAssembly);
                                cfg.Lifetime = ServiceLifetime.Singleton;
                            })
                            ;

                        services
                            .AddControllers()
                            .AddNewtonsoftJson(
                                opts =>
                                {
                                    opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                                    opts.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                                    opts.SerializerSettings.ContractResolver =
                                        new CamelCasePropertyNamesContractResolver();
                                })
                            ;

                        services.AddSwaggerGen(
                                    opts =>
                                    {
                                        opts.ResolveConflictingActions(x => x.First());
                                        opts.DocumentFilter<AreaPathsFixingFilter>();
                                        foreach (var area in config.GetKnownAreas())
                                            opts.SwaggerDoc(area.DocumentName, new OpenApiInfo { Title = area.SwaggerTitle });
                                        foreach (var path in LookupXmlCommentFiles())
                                            opts.IncludeXmlComments(path);
                                    })
                                .AddSwaggerGenNewtonsoftSupport()
                            ;
                    })
                .Configure(
                    (context, appBuilder) =>
                    {
                        var config = context.GetConfig();
                        //var errorHandler = appBuilder.ApplicationServices.GetRequiredService<UniversalExceptionHandler>();

                        appBuilder.Map(
                            config.ApiConfig.RoutePrefix
                            , pipeline =>
                            {
                                pipeline
                                    //.UseExceptionHandler(opts => opts.Run(errorHandler.Invoke))
                                    .UseRouting()
                                    .UseMiddleware<RequestRewindingMiddleware>()
                                    .UseMiddleware<ResponseRewindingMiddleware>()
                                    .UseMiddleware<HttpTracingMiddleware>()
                                    .UseEndpoints(cfg => cfg.MapControllers())
                                    ;
                            });

                        appBuilder.UseSwagger()
                                  .UseSwaggerUI(
                                      cfg =>
                                      {
                                          foreach (var area in config.GetKnownAreas())
                                              cfg.SwaggerEndpoint(area.SwaggerEndpointUrl, area.SwaggerTitle);
                                      });
                    })

                ;

            return webHostBuilder;
        }

        private static WebConfig GetConfig(this WebHostBuilderContext context)
        {
            return context.Configuration.GetSection(nameof(WebConfig)).Get<WebConfig>();
        }

        private static IEnumerable<string> LookupXmlCommentFiles()
        {
            var currentAssembly = typeof(ComponentExtensions).Assembly;
            var rootFolder = Path.GetDirectoryName(currentAssembly.Location);
            var files = new[]
            {
                $"{currentAssembly.GetName().Name}.xml"
            };

            foreach (var file in files)
            {
                var locations = new[] { "", ".." };
                var path = locations.Select(loc => Path.Combine(rootFolder, loc, file))
                                    .OrderByDescending(File.Exists)
                                    .First();

                yield return path;
            }
        }
    }
}