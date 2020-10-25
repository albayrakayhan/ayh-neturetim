using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace aceka.web_api.Models
{
    public class ParameterFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.Request.Method != HttpMethod.Get)
            {
                var idParams = actionContext.ControllerContext.Request.GetRouteData();
                foreach (var item in idParams.Values)
                {
                    object value;
                    var result = actionContext.ActionArguments.TryGetValue(item.Key, out value);
                    if (result)
                    {
                        var parm = actionContext.ActionDescriptor.GetParameters().Where(x => x.ParameterName == item.Key).SingleOrDefault();
                        var type = parm.ParameterType;
                        // Check Action parameter type and convert if needed
                        if (type == typeof(int))
                        {
                            actionContext.ActionArguments[item.Key] = Convert.ToInt32(item.Value);
                        }

                        if (type == typeof(string))
                        {
                            actionContext.ActionArguments[item.Key] = item.Value.ToString();
                        }
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}