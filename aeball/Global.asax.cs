using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using aeball.Models;
using Comlib;

namespace aeball
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string strLogPath = Server.MapPath(ConfigurationManager.AppSettings["LogPath"]); //Server.MapPath("~/Log");            
            LogTrace.SetLogPath(strLogPath);

            Mgn dbMgn = new Mgn();
            SysApp.Initialize(dbMgn);

            //所有的Action都呼叫 SessionExpireFilter
            GlobalFilters.Filters.Add(new SessionExpireFilter());
        }
    }
}
