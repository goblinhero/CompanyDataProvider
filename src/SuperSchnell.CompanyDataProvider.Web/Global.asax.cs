using System;
using System.Web;
using ServiceStack;
using SuperSchnell.CompanyDataProvider.Web.REST;

namespace SuperSchnell.CompanyDataProvider.Web
{
    public class Global : HttpApplication
    {
        public class AppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public AppHost()
                : base("Company data provider", typeof (CompanyService).Assembly)
            {
            }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}