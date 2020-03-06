using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace util.BRLight
{
    public static class JSON
    {

        public static T Deserializa<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            return jss.Deserialize<T>(json);
        }

        public static String Serialize(object objeto)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            StringBuilder stringBuilder = new StringBuilder();
            jss.Serialize(objeto, stringBuilder);
            return stringBuilder.ToString();
        }

        public static string jsonEncode(string val)
        {
            val = val.Replace("\r\n", " ")
                     .Replace("\r", " ")
                     .Replace("\n", " ")
                     .Replace("\"", "\\\"")
                     .Replace("\\", "\\\\");

            return val;
        }

    }
}