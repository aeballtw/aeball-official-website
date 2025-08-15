using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Comlib;

namespace aeball.Models
{
    public class SessionExpireFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            bool NeedCheck = true;

            // check if session is supported
            var route = filterContext.RouteData;

            string strAppPath = HttpContext.Current.Request.ApplicationPath;
            string defaultUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + (strAppPath == "/" ? "" : strAppPath) + "/Home/Login";

            string CName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string AName = filterContext.ActionDescriptor.ActionName;
            CName = CName.ToLower();
            AName = AName.ToLower();
            string AreaName = filterContext.RouteData.DataTokens["Area"] == null ? "" : filterContext.RouteData.DataTokens["Area"].ToString().ToLower();

            /*3-登入狀態*/
            #region 登入狀態            
            if (!IsPublic(CName, AName))
            {
                if (CName == "auth")
                {
                    ctx.Session.Clear();
                }
                else if (filterContext.HttpContext.Session["CurrUser"] == null)
                {
                    LogTrace.Log("不是公開頁面 且未登入 所以登出");
                    _Logout();
                    base.OnActionExecuting(filterContext);
                    return;
                }
                else if (ctx.Session != null)
                {
                    if (ctx.Session.IsNewSession)
                    {
                        LogTrace.Log("不是公開頁面 IsNewSession 所以登出");
                        // If it says it is a new session, but an existing cookie exists, then it must
                        // have timed out
                        string sessionCookie = ctx.Request.Headers["Cookie"];
                        if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                        {
                            _Logout();
                            base.OnActionExecuting(filterContext);
                            return;
                        }
                    }
                }

            }
            #endregion

            //登出
            void _Logout()
            {
                ActionResult result = new ContentResult { Content = "<script>alert('登入逾時!');window.open('" + defaultUrl + "','_top')</script>" };
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.HttpContext.Response.StatusCode = 501;
                filterContext.Result = result;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 是不是公開頁面
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="AName"></param>
        /// <returns></returns>
        private bool IsPublic(string CName, string AName)
        {
            var ispublic = false;
            CName = CName == null ? "" : CName.ToLower();
            AName = AName == null ? "" : AName.ToLower();

            #region UnPublic
            var unpublic_pairs = new List<ActionPair>();
            //unpublic_pairs.Add(new ActionPair() { ControlName = "admin", ActionName = "userpwupdate" });

            #endregion

            #region Public
            var public_pairs = new List<ActionPair>();
            public_pairs.Add(new ActionPair() { ControlName = "home" });
            //public_pairs.Add(new ActionPair() { ControlName = "home" });
            #endregion

            foreach (var pair in public_pairs)
            {
                //Controller、Action都比對
                if (!string.IsNullOrEmpty(pair.ControlName) && !string.IsNullOrEmpty(pair.ActionName))
                {
                    if (CName == pair.ControlName.ToLower() && AName == pair.ActionName.ToLower())
                    {

                        ispublic = true;
                        break;
                    }
                }
                else
                {
                    //只比對Controller
                    if (!string.IsNullOrEmpty(pair.ControlName) && CName == pair.ControlName.ToLower())
                    {
                        ispublic = true;
                        break;
                    }
                    //只比對Action
                    if (!string.IsNullOrEmpty(pair.ActionName) && AName == pair.ActionName.ToLower())
                    {
                        ispublic = true;
                        break;
                    }
                }

            }

            if (ispublic)
            {
                //LogTrace.Log("CName:" + CName + " AName:" + AName + "是公開頁面 接著判斷不能公開的");
                foreach (var pair in unpublic_pairs)
                {
                    if (!string.IsNullOrEmpty(pair.ControlName) && !string.IsNullOrEmpty(pair.ActionName))
                    {
                        if (CName == pair.ControlName.ToLower() && AName == pair.ActionName.ToLower())
                        {
                            //LogTrace.Log("CName:" + CName + " AName:" + AName + "不能公開 因為unpair:" + pair.ControlName.ToLower() + " & " + pair.ActionName.ToLower());
                            ispublic = false;
                            break;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(pair.ControlName) && CName == pair.ControlName.ToLower())
                        {
                            //LogTrace.Log("CName:" + CName + " AName:" + AName + "不能公開 因為unpair:" + pair.ControlName.ToLower());
                            ispublic = false;
                            break;
                        }
                        if (!string.IsNullOrEmpty(pair.ActionName) && AName == pair.ActionName.ToLower())
                        {
                            //LogTrace.Log("CName:" + CName + " AName:" + AName + "不能公開 因為unpair:" + pair.ActionName.ToLower());
                            ispublic = false;
                            break;
                        }
                    }
                }

            }
            //LogTrace.Log("CName:" + CName + " AName:" + AName + ((ispublic) ? "可公開" : "不可公開"));
            return ispublic;
        }

        private class ActionPair
        {
            public string ControlName { get; set; }
            public string ActionName { get; set; }
        }
    }
}