using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;
using LightInject;
using System.Web.Http.Cors;
using MobileApp.Swagger;
using Swashbuckle.Application;

namespace MobileApp
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            // Swagger
            var zumoApiHeader = new SwaggerHeaderParameters
            {
                Description = "The default header for app services defining their version",
                Key = "ZUMO-API-VERSION",
                Name = "ZUMO-API-VERSION",
                DefaultValue = "2.0.0"
            };

            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            httpConfiguration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            //var basePath = AppDomain.CurrentDomain.BaseDirectory;
            httpConfiguration.EnableSwagger(x =>
            {
                x.SingleApiVersion("v1", "ToDoSwagger");
                zumoApiHeader.Apply(x);
                //authHeader.Apply(x);
                //x.IncludeXmlComments(basePath + "\\bin\\ToDoSwagger.XML");
            }).EnableSwaggerUi();

            var container = new ServiceContainer();

            container.RegisterApiControllers();
            container.EnableWebApi(httpConfiguration);

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .MapApiControllers()
                .ApplyTo(httpConfiguration);

            app.UseWebApi(httpConfiguration);
        }
    }
}

