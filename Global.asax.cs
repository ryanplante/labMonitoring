using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace labMonitor
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            DateTime now = DateTime.Now;
            // Log the exception. Probably be better to log it to a database but for now the admin can just check the console
            Console.WriteLine($"{now} : {exception}");
            //Clear the error from the server
            Server.ClearError();
            Response.Redirect("Error.aspx");
        }
    }
}