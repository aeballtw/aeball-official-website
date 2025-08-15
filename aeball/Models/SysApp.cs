using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comlib;

namespace aeball.Models
{
    public class SysApp
    {
        public static string FilesRoot = "";
        private static Mgn _Mgn = null;
        public static void Initialize(Mgn Mgn)
        {
            _Mgn = Mgn;
        }

        public static Mgn DbMgn
        {
            get
            {
                return _Mgn;
            }
        }

        #region 使用者
        public static SysUser CurrUser
        {
            get
            {
                return HttpContext.Current.Session["CurrUser"] == null ? null : (SysUser)HttpContext.Current.Session["CurrUser"];
            }
        }

        /// <summary>
        /// 取得目前登入者ID
        /// </summary>
        /// <returns></returns>
        public static string GetCurrUserID
        {
            get
            {
                if (CurrUser != null) return CurrUser.cuID.ToString();
                return "";
            }
        }
        #endregion
    }
}