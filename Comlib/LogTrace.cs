using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Comlib
{
    public class LogTrace
    {
        private static string _Path { get; set; }
        public static void SetLogPath(string path)
        {
            _Path = path;
            if (!Directory.Exists(_Path))
            {
                Directory.CreateDirectory(_Path);
            }
        }
        public static void Log(string msg, string name = "")
        {
            string fileName = _Path + DateTime.Now.ToString("yyyyMMdd");
            if (!string.IsNullOrEmpty(name))
            {
                fileName = _Path + name;
            }

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName + ".txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " => " + msg);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
