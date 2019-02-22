using Consumer.Models;
using Consumer.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using Consumer.Commands.Collapsers;

namespace Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDiscoveryClient(Configuration);

            services.AddSingleton<IPriceService, PriceService>();

            services.AddHystrixCommand<CategoryServiceCommand>("ServiceGroup", "CategoryServiceCommand", Configuration);
            services.AddHystrixCommand<ProductServiceCommand>("ServiceGroup", "ProductServiceCommand", Configuration);
            services.AddHystrixCommand<StarServiceCommand>("ServiceGroup", "StarServiceCommand", Configuration);

            services.AddHystrixCommand<CurrencyServiceCommand>("CachedServiceGroup", "CurrencyServiceCommand", Configuration);

            services.AddHystrixCollapser<MultiPriceCommandCollapser>("CollapserGroup", Configuration);

            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseHystrixRequestContext();
            app.UseDiscoveryClient();
            app.UseHystrixMetricsStream();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=IndexAsync}/{id?}");
            });
        }
    }
}
