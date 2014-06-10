using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebAPIDemo.Web.Services
{
    public class CountingKsControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        public CountingKsControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //return base.SelectController(request);
            var listOfKnownControllers = GetControllerMapping();
            var routeData = request.GetRouteData();
            var controllerName = (string) routeData.Values["controller"];

            HttpControllerDescriptor descriptor;
            if (listOfKnownControllers.TryGetValue(controllerName, out descriptor))
            {
                const string version = "2";
                var newName = String.Concat(controllerName, "V", version);
                
                HttpControllerDescriptor versionedDescriptor;
                if (listOfKnownControllers.TryGetValue(newName, out versionedDescriptor))
                {
                    return versionedDescriptor;
                }

                return descriptor;
            }

            return null;
        }
    }
}