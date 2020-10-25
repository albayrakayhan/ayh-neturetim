using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using aceka.web_api.Models;
using aceka.web_api.App_Start.Filters;

namespace aceka.web_api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();
        
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        

            //Web API Cors (Cross Origin için)
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        
            var jsonFormatter = new JsonMediaTypeFormatter();
            //optional: set serializer settings here
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
        
        
            // model state error handel
            //config.Filters.Add(new ValidateModelAttribute());

        }
        
    }
}
