using custom_sample_ui.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;

using Steeltoe.CircuitBreaker.Hystrix;
namespace custom_sample_ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDiscoveryClient(Configuration);
            services.AddSingleton<IDummyService, DummyService>();
            services.AddHystrixCommand<DummyServiceCommand>("DummyServiceGroup", "DummyServiceCommand", Configuration);
            services.AddHystrixMetricsStream(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseHystrixRequestContext();
            app.UseDiscoveryClient();
            app.UseHystrixMetricsStream();
        }
    }
}
