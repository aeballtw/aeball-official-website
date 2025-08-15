using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Comlib;
using aeball.Models;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace aeball.Controllers
{
    public class CommonController : Controller
    {
        protected string[] VaildExtension = { ".jpg", ".jpeg", ".png" };
        protected string[] InvalidExtension = { ".js", ".config", ".asp", ".aspx", ".cshtml", ".asax", ".dll", ".cs", ".bat" };
        public string RefillSession()
        {
            return "1";
        }

        public ActionResult Error(string msg, string cusMsg = "")
        {
            ViewData["Msg"] = msg;
            ViewData["cusMsg"] = cusMsg;
            return View("~/Views/Shared/Error.cshtml");
        }

        protected SysUser CurrUser
        {
            get
            {
                return Session["CurrUser"] as SysUser;
            }
        }

        protected SysUser CurrVerifyUser
        {
            get
            {
                return Session["CurrVerifyUser"] as SysUser;
            }
        }

        public string ToStr(string ret, int mgnType = 1)
        {
            string errMsg = getErrMsg(mgnType);
            if (errMsg != "")
            {
                Response.StatusCode = 510;
                Response.Write(errMsg);
                Response.End();
            }
            return ret;
        }

        public string ToJsonStr(object obj, int mgnType = 1)
        {
            string errMsg = getErrMsg(mgnType);
            if (errMsg != "")
            {
                Response.StatusCode = 510;
                Response.Write(errMsg);
                Response.End();
            }
            string ret = "";
            try
            {
                ret = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                ret = "";
            }
            return ret;
        }

        public ActionResult ToJson(Object obj, int mgnType = 0)
        {
            string errMsg = getErrMsg(mgnType);
            if (errMsg != "")
            {
                Response.StatusCode = 510;
                return Content(errMsg);
            }
            string ret = "";
            try
            {
                ret = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                ret = "";
            }
            return Content(ret);
        }

        private string getErrMsg(int mgnType)
        {
            string ret = "";
            switch (mgnType)
            {
                case 1:
                    ret = SysApp.DbMgn.LatestErrMsg;
                    SysApp.DbMgn.LatestErrMsg = "";
                    break;
                case 2:
                    ret = SysApp.DbMgn.LatestErrMsg;
                    SysApp.DbMgn.LatestErrMsg = "";
                    break;
            }
            return ret;
        }

        public bool CheckFileOK(string filepath)
        {
            string ext = Path.GetExtension(filepath).ToLower();
            if (filepath.Contains("../"))
            {
                return false;
            }

            foreach (var invalid in InvalidExtension)
            {
                if (filepath.Contains(invalid))
                {
                    return false;
                }
            }
            return true;
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public bool IsValidPhone(string phone)
        {
            try
            {
                string reg = @"^09([0-9]{8})$";
                if (phone != null && phone.Count() == 10) return Regex.IsMatch(phone, reg);
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (System.FormatException ex)
            {
                return false;
            }
        }

        public static string GetBarCode(string text)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_39;
            EncodingOptions options = new EncodingOptions()
            {
                Height = 50,
                Width = 350,
                Margin = 2
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            for (Int32 y = 0; y < map.Height; y++) { 
                for (Int32 x = 0; x < map.Width; x++)
                {
                    Color PixelColor = map.GetPixel(x, y);
                    if (PixelColor.ToArgb() == Color.White.ToArgb())
                    {
                        map.SetPixel(x, y, Color.FromArgb(255, 244, 234));
                    }
                }
            }
            System.IO.MemoryStream ms = new MemoryStream();
            map.Save(ms, ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);
            return SigBase64;
        }

    }
}