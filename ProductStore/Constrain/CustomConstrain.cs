using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Constrain
{
    public class CustomConstraint : IRouteConstraint
    {
        string[] positions = new[] { "Admin", "SuperAdmin"};
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (Int32.TryParse(values["id"].ToString(), out int id))
            {
                if (id >= 1 && id <= 100)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
