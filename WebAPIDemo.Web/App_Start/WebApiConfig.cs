using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;
using WebAPIDemo.Web.Filter;

namespace WebAPIDemo.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "Food",
               routeTemplate: "api/nutrition/foods/{foodid}",
               defaults: new { controller = "Foods", foodid = RouteParameter.Optional }//,
                //constraints: new { id= "/d+" }
           );

            config.Routes.MapHttpRoute(
              name: "Measures",
              routeTemplate: "api/nutrition/foods/{foodid}/measures/{id}",
              defaults: new { controller = "Measures", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "Diaries",
             routeTemplate: "api/user/diaries/{diaryid}",
             defaults: new { controller = "Diaries", diaryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "DiaryEntries",
             routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
             defaults: new { controller = "DiaryEntries", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "DiarySummary",
             routeTemplate: "api/user/diaries/{diaryid}/summary",
             defaults: new { controller = "DiarySummary" }
            );

            config.Routes.MapHttpRoute(
             name: "Token",
             routeTemplate: "api/token",
             defaults: new { controller = "Token" }
            );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            // formatt to json
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // will change the json properties to camel case.
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            // CROSS ORIGIN SECURITY
            // CALL FROM ANOTHER DOMAIN USING JAVASCRIPT
            // Add Support JSONP
            // http://localhost:23042/api/nutrition/foods?callback=foo
            // will wrap the response in a foo function and thinks its a piece of javascript that needs to be run.
            var formatter = new JsonpMediaTypeFormatter(jsonFormatter, "callback");
            config.Formatters.Insert(0, formatter);

            // Forces HTTPS on entire API
            //#if DEBUG
            //{
            //    config.Filters.Add(new RequireHttpsAttribute());
            //}
            //#endif

            // CORS SUPPORT = CROSS ORIGIN RESOURCE SHARING
            // ONLY AVAILABLE IN Web API 2
            
        }
    }
}
