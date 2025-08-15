using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using aeball.Models;
using Comlib;

namespace aeball.Controllers
{
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Place()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckAccount(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            if (string.IsNullOrEmpty(data.cuID))
            {
                res.Message = "請輸入手機號碼！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuPawd))
            {
                res.Message = "請輸入密碼！";
                return ToJson(res);
            }
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(data.cuID);
            if (user == null || string.IsNullOrEmpty(user.cuTel))
            {
                res.Message = "手機號碼或密碼錯誤！";
                return ToJson(res);
            }
            if (data.cuPawd != user.cuPawd)
            {
                res.Message = "手機號碼或密碼錯誤！";
                return ToJson(res);
            }
            Session["CurrUser"] = user;
            res.RetCode = enReturnValue.Correct;
            return ToJson(res);
        }

        #region 忘記密碼
        public ActionResult ForgetPWD()
        {
            Session["ForgetUser"] = new SysUser();
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult VerifyForgetPwd(string phone)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            if (string.IsNullOrEmpty(phone))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(phone))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            var rand = new Random();
            string verifycode = "";
            for (int i = 0; i < 6; i++)
            {
                verifycode += rand.Next(10).ToString();
            }
            if (Session["ForgetUser"] == null) Session["ForgetUser"] = new SysUser();
            var user = (SysUser)Session["ForgetUser"];
            user.cuTel = phone;
            user.VerifyCode = verifycode;
            user.SmsValid = false;
            Session["ForgetUser"] = user;
            var IsSendSuccess = SysApp.DbMgn.VerifyPhone(phone, verifycode, "2");
            if (IsSendSuccess)
            {
                user.VerfiyTime = DateTime.Now;
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "簡訊傳送錯誤，請再次嘗試!";
            }
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult IdentifyForgetPwd(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            var user = (SysUser)Session["ForgetUser"];
            if (string.IsNullOrEmpty(data.cuTel))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(data.cuTel))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(user.cuTel))
            {
                res.Message = "請先進行簡訊驗證!";
                return ToJson(res);
            }
            if (user.cuTel != data.cuTel)
            {
                res.Message = "與驗證的手機號碼不同!";
                return ToJson(res);
            }
            else if (user.VerfiyTime == null || user.VerfiyTime.AddMinutes(15.0) < DateTime.Now)
            {
                res.Message = "驗證碼逾時!";
                return ToJson(res);
            }
            var temp = SysApp.DbMgn.QueryCusts(data.cuTel);
            if (string.IsNullOrEmpty(data.VerifyCode))
            {
                res.Message = "請輸入簡訊驗證碼!";
                LogTrace.Log("簡訊驗證碼輸入錯誤! IP：" + GetIPAddress());
                return ToJson(res);
            }
            if (temp == null || string.IsNullOrEmpty(temp.cuID))
            {
                res.Message = "手機好碼或是簡訊驗證碼錯誤!";
                return ToJson(res);
            }
            if (data.VerifyCode != user.VerifyCode)
            {
                res.Message = "手機好碼或是簡訊驗證碼錯誤!";
                return ToJson(res);
            }
            user.cuID = temp.cuID;
            user.SmsValid = true;
            Session["ForgetUser"] = user;
            res.RetCode = enReturnValue.Correct;
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckForgetPWDVerifyCode(string code)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            var user = (SysUser)Session["ForgetUser"] ?? new SysUser();
            if (string.IsNullOrEmpty(code))
            {
                res.Message = "請輸入驗證碼!";
            }
            else if (string.IsNullOrEmpty(user.VerifyCode))
            {
                res.Message = "請發送驗證碼!";
            }
            else if (user.VerifyCode != code)
            {
                res.Message = "驗證碼輸入錯誤!";
            }
            else if (user.VerfiyTime == null || user.VerfiyTime.AddMinutes(15.0) < DateTime.Now)
            {
                res.Message = "驗證碼逾時!";
            }
            else if (user.VerifyCode == code)
            {
                res.Message = "驗證成功!";
            }
            return ToJson(res);
        }

        public ActionResult ForgetPWD2()
        {
            if (Session["ForgetUser"] == null) Session["ForgetUser"] = new SysUser();
            var user = (SysUser)Session["ForgetUser"];
            if (string.IsNullOrEmpty(user.cuTel) || !user.SmsValid)
            {
                return RedirectToAction("ForgetPWD");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult SavePwd(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            var user = (SysUser)Session["ForgetUser"];
            if (user == null || string.IsNullOrEmpty(user.cuTel) || user.SmsValid == false)
            {
                res.Message = "請重新進行簡訊驗證!";
                return ToJson(res);
            }
            var temp = SysApp.DbMgn.QueryCusts(user.cuTel);
            if (temp == null || string.IsNullOrEmpty(temp.cuTel))
            {
                res.Message = "手機號碼或是簡訊驗證碼有誤!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuPawd))
            {
                res.Message = "請輸入密碼!";
                return ToJson(res);
            }
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,18}$");
            if (!regex.IsMatch(data.cuPawd))
            {
                res.Message = "密碼長度必須是6-18，含英數字!";
                return ToJson(res);
            }
            temp.cuPawd = data.cuPawd;
            temp.modifyDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            if (SysApp.DbMgn.UpdateCusts(temp))
            {
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員!";
            }

            return ToJson(res);
        }

        #endregion


        public ActionResult Logout()
        {
            Session["CurrUser"] = null;
            System.Web.HttpContext.Current.Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        #region 註冊
        public ActionResult Register()
        {
            if (Session["RegistUser"] == null) Session["RegistUser"] = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult VerifyPhone(string phone)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            if (string.IsNullOrEmpty(phone))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(phone))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            var rand = new Random();
            string verifycode = "";
            for (int i = 0; i < 6; i++)
            {
                verifycode += rand.Next(10).ToString();
            }
            var user = (SysUser)Session["RegistUser"]??new SysUser();
            user.cuTel = phone;
            user.VerifyCode = verifycode;
            user.SmsValid = false;
            Session["RegistUser"] = user;
            var IsSendSuccess = SysApp.DbMgn.VerifyPhone(phone, verifycode);
            if (IsSendSuccess)
            {
                user.VerfiyTime = DateTime.Now;
                res.RetCode = enReturnValue.Correct;
                //res.Message = "Code：" + verifycode;
            }
            else
            {
                res.Message = "簡訊傳送錯誤，請再次嘗試!";
            }
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckPhone(string phone)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            if (string.IsNullOrEmpty(phone))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(phone))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(phone);
            if (user != null && !string.IsNullOrEmpty(user.cuTel))
            {
                res.Message = "手機號碼已註冊，請直接登入!";
                return ToJson(res);
            }
            else
            {
                res.RetCode = enReturnValue.Correct;
            }
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckLoginPhone(string phone)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            if (string.IsNullOrEmpty(phone))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(phone))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(phone);
            if (user == null || string.IsNullOrEmpty(user.cuTel))
            {
                res.Message = "此手機號碼尚未註冊!";
                return ToJson(res);
            }
            else
            {
                res.RetCode = enReturnValue.Correct;
            }
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CheckVerifyCode(string code)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            var user = (SysUser)Session["RegistUser"] ?? new SysUser();
            if (string.IsNullOrEmpty(code))
            {
                res.Message = "請輸入驗證碼!";
            }
            else if (string.IsNullOrEmpty(user.VerifyCode))
            {
                res.Message = "請發送驗證碼!";
            }
            else if (user.VerfiyTime == null || user.VerfiyTime.AddMinutes(15.0) < DateTime.Now)
            {
                res.Message = "驗證碼逾時!";
            }
            else if(user.VerifyCode != code)
            {
                res.Message = "驗證碼輸入錯誤!";
            }
            else if (user.VerifyCode == code)
            {
                res.Message = "驗證成功!";
            }
            return ToJson(res);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GetCusts(string phone = "-1")
        {
            var list = SysApp.DbMgn.QueryCusts(phone);
            return ToJson(list);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SavePhone(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            if (string.IsNullOrEmpty(data.cuTel))
            {
                res.Message = "請輸入手機號碼!";
                return ToJson(res);
            }
            if (!IsValidPhone(data.cuTel))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(user.cuTel))
            {
                res.Message = "請先進行簡訊驗證!";
                return ToJson(res);
            }
            if (user.cuTel != data.cuTel)
            {
                res.Message = "與驗證的手機號碼不同!";
                return ToJson(res);
            }
            if (user.VerfiyTime == null || user.VerfiyTime.AddMinutes(15.0) < DateTime.Now)
            {
                res.Message = "驗證碼逾時!";
                return ToJson(res);
            }
            var temp = SysApp.DbMgn.QueryCusts(data.cuTel);
            if(temp != null && !string.IsNullOrEmpty(temp.cuID))
            {
                res.Message = "手機號碼已申請!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.VerifyCode))
            {
                res.Message = "請輸入簡訊驗證碼!";
                LogTrace.Log("簡訊驗證碼輸入錯誤! IP：" + GetIPAddress());
                return ToJson(res);
            }
            if (data.VerifyCode != user.VerifyCode)
            {
                res.Message = "簡訊驗證碼錯誤!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuPawd))
            {
                res.Message = "請輸入密碼!";
                return ToJson(res);
            }
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,18}$");
            if (!regex.IsMatch(data.cuPawd))
            {
                res.Message = "密碼長度必須是6-18，含英數字!";
                return ToJson(res);
            }
            user.cuPawd = data.cuPawd;
            user.SmsValid = true;
            Session["RegistUser"] = user;
            res.RetCode = enReturnValue.Correct;
            return ToJson(res);
        }

        public ActionResult RegisterUser()
        {
            if (Session["RegistUser"] == null) Session["RegistUser"] = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            if(string.IsNullOrEmpty(user.cuTel) || string.IsNullOrEmpty(user.cuPawd) || !user.SmsValid)
            {
                return RedirectToAction("Register");
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaveUser(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            if (string.IsNullOrEmpty(user.cuTel))
            {
                res.RetCode = enReturnValue.NotCorrect;
                res.Message = "請重新申請!";
                return ToJson(res);
            }
            var temp = SysApp.DbMgn.QueryCusts(user.cuTel);
            if (temp != null && !string.IsNullOrEmpty(temp.cuID))
            {
                res.Message = "手機號碼已申請!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuName))
            {
                res.Message = "請輸入姓名!";
                return ToJson(res);
            }
            if (!string.IsNullOrEmpty(data.cuMail) && !string.IsNullOrEmpty(data.cuMail) && !string.IsNullOrEmpty(data.cuBirthday) && data.cuCity != 0 && data.cuTown != 0 && new List<string>() { "0", "1", "2" }.Any(a=>a == data.cuGender))
            {
                user.cuDots = 30;
            }
            user.cuName = data.cuName;
            user.cuBirthday = string.IsNullOrEmpty(data.cuBirthday) ? "" : data.cuBirthday;
            user.cuGender = string.IsNullOrEmpty(data.cuGender) ? "" : data.cuGender;
            user.cuCity = data.cuCity;
            user.cuTown = data.cuTown;
            user.cuArea = string.IsNullOrEmpty(data.cuArea) ? "" : data.cuArea;
            user.cuMail = string.IsNullOrEmpty(data.cuMail) ? "" : data.cuMail;
            if (!new List<string>() { "0", "1", "2" }.Any(a => a == data.cuGender)) data.cuGender = "";
            Session["RegistUser"] = user;
            res.RetCode = enReturnValue.Correct;
            return ToJson(res);
        }

        public ActionResult RegisterPin()
        {
            if (Session["RegistUser"] == null) Session["RegistUser"] = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            if (string.IsNullOrEmpty(user.cuTel) || string.IsNullOrEmpty(user.cuPawd) || !user.SmsValid)
            {
                return RedirectToAction("Register");
            }
            if (string.IsNullOrEmpty(user.cuName))
            {
                return RedirectToAction("RegisterUser");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult SavePin(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            var user = (SysUser)Session["RegistUser"];
            if (string.IsNullOrEmpty(data.cuPin))
            {
                res.RetCode = enReturnValue.NotCorrect;
                res.Message = "請輸入Pin碼!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(user.cuTel))
            {
                res.RetCode = enReturnValue.NotCorrect;
                res.Message = "請重新申請!";
                return ToJson(res);
            }
            var temp = SysApp.DbMgn.QueryCusts(user.cuTel);
            if (temp != null && !string.IsNullOrEmpty(temp.cuID))
            {
                res.Message = "手機號碼已申請!";
                return ToJson(res);
            }
            if (!IsValidPhone(user.cuTel))
            {
                res.Message = "手機號碼格式不對!";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(user.cuName))
            {
                res.Message = "缺少使用者姓名!";
                return ToJson(res);
            }

            var rand = new Random();
            string QRCode = "";
            string text = "0123456789abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < 5; i++)
            {
                QRCode += text[rand.Next(36)].ToString().ToUpper();
            }
            //使用者+1
            int readText = int.Parse(System.IO.File.ReadAllLines(Server.MapPath("~/data/user.txt")).FirstOrDefault());
            using (StreamWriter outputFile = new StreamWriter(Server.MapPath("~/data/user.txt")))
            {
                outputFile.WriteLine(readText+1);
            }
            QRCode += (readText%100000).ToString().PadLeft(5, '0');

            user.cuPin = data.cuPin;
            user.cuID = user.cuTel;
            user.cuQR = QRCode;
            user.token = "";
            user.cuBad = "";
            user.cuEnd = "";
            user.cuLastD = "";
            user.modifyDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            user.modifyStatus = "";
            user.createDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            Session["RegistUser"] = user;
            if (SysApp.DbMgn.InsertCusts(user))
            {
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員!";
            }

            SysUser loginuser = (SysUser)SysApp.DbMgn.QueryCusts(user.cuID);
            if (loginuser == null || string.IsNullOrEmpty(loginuser.cuTel))
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員!!";
                return ToJson(res);
            }
            Session["CurrUser"] = loginuser;
            return ToJson(res);
        }

        public ActionResult RegisterSuccess()
        {
            if(SysApp.CurrUser == null || string.IsNullOrEmpty(SysApp.CurrUser.cuID))
            {
                return RedirectToAction("Register");
            }
            return View();
        }

        #endregion

    }
}