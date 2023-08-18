using Identity.Shared;
using Identity.Web.Utils.Web.Middlewares.Rewinding;
using Identity.Web.Utils.Web.Middlewares.Tracing;
using Identity.Web.Utils.Web.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Identity.Web.Utils
{
    public static class ComponentExtensions
    {
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
                            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                {
                                    // указывает, будет ли валидироваться издатель при валидации токена
                                    ValidateIssuer = true,
                                    // строка, представляющая издателя
                                    ValidIssuer = AuthOptions.ISSUER,
                                    //// будет ли валидироваться потребитель токена
                                    ValidateAudience = false,
                                    //// установка потребителя токена
                                    //ValidAudience = AuthOptions.AUDIENCE,
                                    // будет ли валидироваться время существования
                                    ValidateLifetime = true,
                                    // установка ключа безопасности
                                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                                    // валидация ключа безопасности
                                    ValidateIssuerSigningKey = true,
                                };
                            })
                            ;

                        services
                            .AddControllers()
                            .AddNewtonsoftJson(
                                opts =>
                                {
                                    opts.SerializerSettings.Formatting = Formatting.Indented;
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

                                        opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                        {
                                            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                                                  Enter 'Bearer' [space] and then your token in the text input below.
                                                  \r\n\r\nExample: 'Bearer 12345abcdef'",
                                            Name = "Authorization",
                                            In = ParameterLocation.Header,
                                            Type = SecuritySchemeType.ApiKey,
                                            Scheme = JwtBearerDefaults.AuthenticationScheme
                                        });

                                        opts.AddSecurityRequirement(new OpenApiSecurityRequirement
                                        {
                                            {
                                                new OpenApiSecurityScheme
                                                {
                                                    Reference = new OpenApiReference
                                                    {
                                                        Type = ReferenceType.SecurityScheme,
                                                        Id = JwtBearerDefaults.AuthenticationScheme
                                                    }
                                                },
                                                new string[]{}
                                            }
                                        });
                                        ;
                                    })
                                .AddSwaggerGenNewtonsoftSupport()
                            ;
                    })
                .Configure(
                    (context, appBuilder) =>
                    {
                        var config = context.GetConfig();
                        //var errorHandler = appBuilder.ApplicationServices.GetRequiredService<ExceptionHandlerBase>();

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
                                    .UseAuthentication()
                                    .UseAuthorization()
                                    .UseMiddleware<AuthMiddleware>()
                                    .UseEndpoints(cfg => cfg.MapControllers())
                                    ;
                            });


                        appBuilder.UseSwagger()
                                  .UseSwaggerUI(
                                      cfg =>
                                      {
                                          foreach (var area in config.GetKnownAreas())
                                              cfg.SwaggerEndpoint(area.SwaggerEndpointUrl, area.SwaggerTitle);
                                      })
                                  ;
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