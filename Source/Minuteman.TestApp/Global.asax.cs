namespace Minuteman.TestApp
{
    using System;
    using System.Web;
    using System.Web.Routing;

    public class Global : HttpApplication
    {
        private static readonly DataPublisher Publisher = new DataPublisher();

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Publisher.Dispose();
        }
    }
}