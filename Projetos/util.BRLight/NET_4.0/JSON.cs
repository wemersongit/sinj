using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace util.BRLight
{
    public static class JSON
    {
		
        public static T Deserializa<T>(string conteudo) 
        {
		    try {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(conteudo));
                var jsonConvert = new DataContractJsonSerializer(typeof(T));
                return (T)jsonConvert.ReadObject(ms);
			} catch  {
                try{
                    return new JavaScriptSerializer().Deserialize<T>(conteudo);
                }
                catch (Exception ex)
                {
                    throw new Exception("Deserializa JavaScriptSerializer: ", ex);
                }
			}
        }

        public static String Serialize(object data)
        {
            try {
                var serializer = new DataContractJsonSerializer(data.GetType());
                var ms = new MemoryStream();
                serializer.WriteObject(ms, data);
                return Encoding.UTF8.GetString(ms.ToArray());
            }catch {
                try {
                    return new JavaScriptSerializer().Serialize(data);
                } catch (Exception ex) {
                    throw new Exception("JavaScriptSerializer Object: ", ex);
                }
               
            }
        }
     
        public static String Serialize<T>(T t)
        {
		    string jsonString;

            try {
                jsonString = new JavaScriptSerializer().Serialize(t);
            } catch {
                var ser = new DataContractJsonSerializer(typeof(T));
                var ms = new MemoryStream();
                try {
                        ser.WriteObject(ms, t);
                        jsonString = Encoding.UTF8.GetString(ms.ToArray());
                        var regex = new Regex("\"__type\":\"([^\\\"]|\\.)*\",", RegexOptions.Singleline);
                        jsonString = regex.Replace(jsonString, "");
                }
                catch (Exception ex)
                {
                    throw new Exception("JavaScriptSerializer depois DataContractJsonSerializer: ", ex);
                }
                finally
                {
                    ms.Close();
                }
            }

            return jsonString;
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

        public static bool IsJson(string json)
        {
            json = json.Trim();
            return (json.StartsWith("{") && json.EndsWith("}")) || (json.StartsWith("[") && json.EndsWith("]"));
        }

     }
}