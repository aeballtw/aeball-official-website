using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using aeball.Models;
using Comlib;

namespace aeball.Controllers
{
    public class MemberController : CommonController
    {
        // GET: Member
        public ActionResult Index()
        {
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(CurrUser.cuID);
            if (string.IsNullOrEmpty(CurrUser.cuID) || string.IsNullOrEmpty(user.cuID))
            {
                return RedirectToAction("Login", "Home");
            }
            user.QRPic = (string.IsNullOrEmpty(user.cuQR) ? "" : GetBarCode(user.cuQR));
            return View(user);
        }

        public ActionResult ModifyUser()
        {
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(CurrUser.cuID);
            if(string.IsNullOrEmpty(CurrUser.cuID) || string.IsNullOrEmpty(user.cuID))
            {
                return RedirectToAction("Login", "Home");
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaveUser(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            Cust user = SysApp.DbMgn.QueryCusts(CurrUser.cuTel);
            if (user == null || string.IsNullOrEmpty(user.cuID))
            {
                res.Message = "查無使用者！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuName))
            {
                res.Message = "請輸入姓名！";
                return ToJson(res);
            }
            if (user.cuCity!=0 && data.cuCity == 0)
            {
                res.Message = "請選擇行政區！";
                return ToJson(res);
            }
            if (user.cuTown != 0 && data.cuTown == 0)
            {
                res.Message = "請選擇村里！";
                return ToJson(res);
            }
            if (!string.IsNullOrEmpty(user.cuArea) && string.IsNullOrEmpty(data.cuArea))
            {
                res.Message = "請輸入地址！";
                return ToJson(res);
            }
            if (!string.IsNullOrEmpty(user.cuMail) && string.IsNullOrEmpty(data.cuMail))
            {
                res.Message = "請輸入信箱！";
                return ToJson(res);
            }
            if (!string.IsNullOrEmpty(data.cuMail) && !IsValidMail(data.cuMail))
            {
                res.Message = "信箱格式錯誤！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(user.cuGender) || user.cuCity == 0 || user.cuTown == 0 || string.IsNullOrEmpty(user.cuMail) || string.IsNullOrEmpty(user.cuArea) || string.IsNullOrEmpty(user.cuBirthday))
            {
                if (new List<string>() { "0", "1", "2" }.Any(a => a == data.cuGender) && data.cuCity != 0 && data.cuTown != 0 && !string.IsNullOrEmpty(data.cuMail) && !string.IsNullOrEmpty(data.cuArea) && !string.IsNullOrEmpty(data.cuBirthday))
                {
                    user.cuDots += 30;
                }
            }

            user.cuName = data.cuName;
            if (new List<string>() { "0", "1", "2" }.Any(a => a == data.cuGender)) user.cuGender = data.cuGender;
            if (data.cuCity != 0) user.cuCity = data.cuCity;
            if (data.cuTown != 0) user.cuTown = data.cuTown;
            if (!string.IsNullOrEmpty(data.cuArea)) user.cuArea = data.cuArea;
            if (!string.IsNullOrEmpty(data.cuMail)) user.cuMail = data.cuMail;
            if (string.IsNullOrEmpty(user.cuBirthday) && !string.IsNullOrEmpty(data.cuBirthday)) user.cuBirthday = data.cuBirthday;
            user.modifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (SysApp.DbMgn.UpdateCusts(user))
            {
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員！";
            }
            return ToJson(res);
        }

        public ActionResult ModifyPwd()
        {
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(CurrUser.cuID);
            if (string.IsNullOrEmpty(CurrUser.cuID) || string.IsNullOrEmpty(user.cuID))
            {
                return RedirectToAction("Login", "Home");
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SavePwd(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            Cust user = SysApp.DbMgn.QueryCusts(CurrUser.cuTel);
            if (user == null || string.IsNullOrEmpty(user.cuID))
            {
                res.Message = "查無使用者！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.OldPwd))
            {
                res.Message = "請輸入舊密碼！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuPawd))
            {
                res.Message = "請輸入密碼！";
                return ToJson(res);
            }
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,18}$");//(?=.*\W)
            if (!regex.IsMatch(data.cuPawd))
            {
                res.Message = "密碼長度必須是6-18位，含大小寫英數字！";
                return ToJson(res);
            }
            if (data.OldPwd != user.cuPawd)
            {
                res.Message = "原密碼輸入錯誤";
                return ToJson(res);
            }
            user.cuPawd = data.cuPawd;
            user.modifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (SysApp.DbMgn.UpdateCusts(user))
            {
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員！";
            }
            return ToJson(res);
        }

        public ActionResult ModifyPin()
        {
            SysUser user = (SysUser)SysApp.DbMgn.QueryCusts(CurrUser.cuID);
            if (string.IsNullOrEmpty(CurrUser.cuID) || string.IsNullOrEmpty(user.cuID))
            {
                return RedirectToAction("Login", "Home");
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SavePin(SysUser data)
        {
            ApiResult res = new ApiResult() { RetCode = enReturnValue.Exception };
            SysUser qmodel = new SysUser();
            Cust user = SysApp.DbMgn.QueryCusts(CurrUser.cuTel);
            if (user == null || string.IsNullOrEmpty(user.cuID))
            {
                res.Message = "查無使用者！";
                return ToJson(res);
            }
            if (string.IsNullOrEmpty(data.cuPin))
            {
                res.Message = "請輸入 PIN 碼！";
                return ToJson(res);
            }
            user.cuPin = data.cuPin;
            user.modifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (SysApp.DbMgn.UpdateCusts(user))
            {
                res.RetCode = enReturnValue.Correct;
            }
            else
            {
                res.Message = "系統儲存失敗，請聯繫系統管理員！";
            }
            return ToJson(res);
        }

    }
}