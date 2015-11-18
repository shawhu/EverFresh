using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace EverFresh_API
{
    public class Global : System.Web.HttpApplication
    {
        public class AppHost : AppHostBase
        {
            //Tell ServiceStack the name of your application and where to find your services
            public AppHost() : base("EverFresh Web Services", typeof(Main_Service).Assembly) { }
            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ServiceStack.Licensing.RegisterLicense(@"2587-e1JlZjoyNTg3LE5hbWU6U2hhbmdoYWkgSEVIRU5HIEludGV
sbGlnZW5jZSBUZWNobmljYWwgQ29tcGFueSBMVEQsVHlwZTpJbmRpZSxIYXNoOm43QU9DSmdEbTZxN2ZmM3lLQStIU2d6bVdpMjlWSUk5
TytzWEoxSnhxT1ZkOWhPelNac2hwZldkblZtd0xjQXJNNVJOV1pxYnN3NlpOQWppNVhQd1hDbDJKOUlGeG0zM0VoVzFqc3FiYWhEVXRCU
WVVb3l3c0xJWWFVbzRKSXZNaDNDblFLdldvcFZWUnppZnFKNDhXeithVWxUOXEycjdpTHZFdDJPRVVNbz0sRXhwaXJ5OjIwMTYtMDUtMTF9");
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