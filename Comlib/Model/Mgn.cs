using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Comlib
{
    public class Mgn
    {
        private string _LatestErrMsg = "";

        public string LatestErrMsg
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session["CurrMsg"] != null)
                    return System.Web.HttpContext.Current.Session["CurrMsg"].ToString();
                return this._LatestErrMsg;
            }
            set
            {
                this._LatestErrMsg = value;
            }
        }

        public SysUser QueryCusts(string phone)
        {
            String uriAPI = ConfigurationManager.AppSettings["aeball_Url"] + "/Data/QryCust";
            List<SysUser> list = new List<SysUser>();
            SysUser result = new SysUser();
            string api_result = "";
            string data = "";
            var dataJson = "{\"QTXT\": \"" + phone + "\"}";
            byte[] dataStream = Encoding.UTF8.GetBytes(dataJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAPI + data);

            try
            {
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ProtocolVersion = HttpVersion.Version10;
                request.Headers.Add("XApiKey", ConfigurationManager.AppSettings["aeball_API_KEY"]);
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();

                using (var reqStream = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(reqStream.GetResponseStream(), Encoding.UTF8))
                    {
                        api_result = reader.ReadToEnd();
                        LogTrace.Log("取得資料json");
                    }
                }


                list = JsonConvert.DeserializeObject<List<SysUser>>(api_result)?? new List<SysUser>();
                result = list.Where(x => x.cuID == phone).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogTrace.Log("GetCusts API 取得錯誤：" + ex.Message) ;
            }
            return result;
        }

        public bool VerifyPhone(string phone, string code, string type = "1")
        {
            String uriAPI = ConfigurationManager.AppSettings["SendText_Url"] + "/api/mtk/SmSend?CharsetURL=UTF-8";
            bool result = false;
            string api_result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAPI);

            StringBuilder data = new StringBuilder("");
            data.Append("username=94196739SMS");
            data.Append("&password=Wearethebest@2023");
            data.Append("&dstaddr=" + phone);
            if (type == "1") data.Append("&smbody=" + code + " 是您的 aeball 驗證碼，請於 15 分鐘內輸入至簡訊驗證碼欄位。歡迎您加入 aeball 享受全新運動體驗！");
            else data.Append("&smbody= 您的 aeball 密碼變更驗證碼為 " + code + " ，有效期限為 15 分鐘，請於時限內完成密碼變更。");
            byte[] dataStream = Encoding.UTF8.GetBytes(data.ToString());
            LogTrace.Log("【aeball系統通知】您的簡訊認證碼為 " + code + " !");

            try
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = dataStream.Length;
                request.GetRequestStream().Write(dataStream, 0, dataStream.Length);

                using (var reqStream = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(reqStream.GetResponseStream(), Encoding.UTF8))
                    {
                        api_result = reader.ReadToEnd();
                        LogTrace.Log("取得資料json：" + api_result);
                        if (api_result.Contains("statuscode=1")) result = true;
                        else result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogTrace.Log("VerifyPhone API 取得錯誤：" + ex.Message);
                result = false;
            }
            return result;
        }

        public bool InsertCusts(SysUser user)
        {
            String uriAPI = ConfigurationManager.AppSettings["aeball_Url"] + "/Data/NewCust";
            SysUser result = new SysUser();
            string api_result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAPI);
            var dataJson = JsonConvert.SerializeObject(user);
            byte[] dataStream = Encoding.UTF8.GetBytes(dataJson);

            try
            {
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ProtocolVersion = HttpVersion.Version10;
                request.Headers.Add("XApiKey", ConfigurationManager.AppSettings["aeball_API_KEY"]);
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();

                using (var reqStream = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(reqStream.GetResponseStream(), Encoding.UTF8))
                    {
                        api_result = reader.ReadToEnd();
                        LogTrace.Log("取得資料json：" + api_result);
                    }
                }
                result = JsonConvert.DeserializeObject<SysUser>(api_result) ?? new SysUser();
            }
            catch (Exception ex)
            {
                LogTrace.Log("InsertCusts API 錯誤：" + ex.Message);
                return false;
            }
            return !string.IsNullOrEmpty(result.cuID)? true: false;
        }

        public bool UpdateCusts(Cust user)
        {
            String uriAPI = ConfigurationManager.AppSettings["aeball_Url"] + "/Data/PutCust";
            string api_result = "";
            bool result = false;
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAPI);
            var dataJson = "[" + JsonConvert.SerializeObject(user) + "]";
            byte[] dataStream = Encoding.UTF8.GetBytes(dataJson);

            try
            {
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.ProtocolVersion = HttpVersion.Version10;
                request.Headers.Add("XApiKey", ConfigurationManager.AppSettings["aeball_API_KEY"]);
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
                using (var reqStream = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(reqStream.GetResponseStream(), Encoding.UTF8))
                    {
                        api_result = reader.ReadToEnd();
                        LogTrace.Log("取得資料json：" + api_result);
                    }
                }
                List<SysUser> list = JsonConvert.DeserializeObject<List<SysUser>>(api_result) ?? new List<SysUser>();
                var temp = list.Where(x => x.cuID == user.cuID).FirstOrDefault();
                if(temp!=null && temp.modifyStatus == "Update Success") result = true;
                else result = false;
            }
            catch (Exception ex)
            {
                LogTrace.Log("UpdateCusts API 取得錯誤：" + ex.Message);
                result = false;
            }
            return result;
        }
    }
}
