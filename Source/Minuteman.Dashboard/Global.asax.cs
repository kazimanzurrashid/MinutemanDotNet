namespace Minuteman.Dashboard
{
    using System;
    using System.Web;
    using System.Web.Routing;

    public class Application : HttpApplication
    {
        private static readonly DataPublisher Publisher = new DataPublisher();

        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Publisher.Dispose();
        }
    }
}