using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrafficInfoHttpApi.Services;

namespace TrafficInfoHttpApi
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
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowedOrigins", builder =>
                {
                    builder.WithOrigins(
                        "http://localhost:4200",
                        "http://planesui.azurewebsites.net",
                        "https://planesui.azurewebsites.net/pages/maps/bubble"
                    );
                });
            });

            services.AddSingleton<AirTrafficInfoService>();
            services.AddSingleton<IAirTrafficInfoService, AirTrafficInfoService>(s => s.GetService<AirTrafficInfoService>());
            services.AddSingleton<StaticResourcesProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO: to extension ? Microsoft.Extensions.Hosting.Environments.
            //extension everything topic
            //https://stackoverflow.com/questions/619033/does-c-sharp-have-extension-properties
            //keeping hardcoded 'Docker' for now
            if (env.IsEnvironment("Docker"))
            {

            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("MyAllowedOrigins");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
