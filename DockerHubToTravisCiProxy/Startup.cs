namespace DockerHubToTravisCiProxy
{
    using Controllers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Serialization;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables("TRAVISCIPROXY_");
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                .Configure<TravisCiIntegrationOptions>(
                    x =>
                    {
                        x.ApiKey = Configuration[nameof(TravisCiIntegrationOptions.ApiKey)];
                        x.ImageValidatorBranch = Configuration[nameof(TravisCiIntegrationOptions.ImageValidatorBranch)];
                    });

            services.AddMvc()
                .AddJsonOptions(x =>
                                {
                                    x.SerializerSettings.ContractResolver =
                                        new DefaultContractResolver(true)
                                        {
                                            NamingStrategy = new SnakeCaseNamingStrategy(true, true)
                                        };
                                });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"));

            app.UseMvc();
        }
    }
}
