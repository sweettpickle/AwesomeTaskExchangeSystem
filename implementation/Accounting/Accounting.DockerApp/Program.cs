using Accounting.Application.Utils;
using Accounting.Integration.Utils;
using Accounting.Persistence.Utils;
using Accounting.Web.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Accounting.DockerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, configurationBuilder) =>
                {
                    configurationBuilder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true)
                        .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(
                    (hostBuilderContext, services) =>
                    {
                        services.AddApplication(hostBuilderContext.Configuration)
                                .AddPersistence(hostBuilderContext.Configuration)
                                .AddIntegration(hostBuilderContext.Configuration)
                            ;
                    })
                .UseSerilog((hostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.AddWeb();
                })
        ;
    }
}
