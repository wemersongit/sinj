using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;


namespace util.BRLight
{

    public static class Util
    {

        public static string GetUA()
        {
            try
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            }
            catch
            {
                return "Agente Indefinido";
            }
        }

        public static string GetUser()
        {
            try
            {
                var user = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
                if (string.IsNullOrEmpty(user)) {
                    user = HttpContext.Current.Request.ServerVariables["LOGON_USER"];
                }
                if (string.IsNullOrEmpty(user))
                {
                    var p = System.Threading.Thread.CurrentPrincipal as WindowsPrincipal;
                    if (p != null) user = p.Identity.Name;
                }
                if (string.IsNullOrEmpty(user)) {
                    user = WindowsIdentity.GetCurrent().Name.ToString();
                }
                if (string.IsNullOrEmpty(user)) {
                   user = HttpContext.Current.User.Identity.Name.ToString();
                }
                return user;
            }
            catch
            {
                try
                {
                    return WindowsIdentity.GetCurrent().Name.ToString();
                }
                catch
                {
                    return "Erro User";
                }
            }
        }

        public static string GetUserIp()
        {

            try {
                var ipAddress =  HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress)) {
                    ipAddress =  HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return string.Concat("{\"ip\":[\"", ipAddress, "\"]}");
            } catch {
                try {
                    return string.Concat("{\"ip\":[\"", HttpContext.Current.Request.UserHostAddress, "\"]}");
                } catch {
                    return "{\"ip\":[]}";
                }
            }

        }

        public static string GetInfoServerApp()
        {
            string ambienteSigla;
            string shortIp;
            string versao;
            string port;
            try
            {
                string ambiente = Config.ValorChave("Ambiente", false);
                switch (ambiente)
                {
                    case "Desenvolvimento":
                        ambienteSigla = "D";
                        break;
                    case "Mono":
                        ambienteSigla = "M";
                        break;
                    case "Light_Teste":
                        ambienteSigla = "LT";
                        break;
                    case "PGFN_Teste":
                        ambienteSigla = "PT";
                        break;
					case "PGFN_Treinamento":
						ambienteSigla = "PTR";
						break;
                    case "PGFN_Homologacao":
                        ambienteSigla = "PH";
                        break;
                    case "PGFN_Producao":
                        ambienteSigla = "PP";
                        break;
                    case "Light_VM_RH":
                        ambienteSigla = "VM_RH";
                        break;
                    default:
                        ambienteSigla = "";
                        break;
                }
            }
            catch
            {
                ambienteSigla = "";
            }
            try
            {
                string ip = Util.GetServerIp();
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.LastIndexOf('.') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf('.')).Replace(".", "");
                    }
                    else if (ip.LastIndexOf(':') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf(':')).Replace(":", "");
                    }
                    else if (ip.LastIndexOf('/') > -1)
                    {
                        shortIp = ip.Substring(ip.LastIndexOf('/')).Replace("/", "");
                    }
                    else
                    {
                        shortIp = "";
                    }
                }
                else
                {
                    shortIp = "";
                }
            }
            catch
            {
                shortIp = "";
            }
            try
            {
                versao = Config.ValorChave("versao");
            }
            catch
            {
                versao = "";
            }
            try
            {
                port = GetServerPort();
            }
            catch
            {
                port = "";
            }
            return ambienteSigla + "." + versao + "." + shortIp + ":" + port;
        }

        public static string GetServerIp()
        {
            try
            {
                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }
            catch
            {
                return "";
            }
        }

        public static string GetServerPort()
        {
            try
            {
                return HttpContext.Current.Request.Url.Port.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string Variables(string _variables) {
            try
            {
                return HttpContext.Current.Request.ServerVariables[_variables];
            }
            catch 
            {
                return _variables + " - Não identificado";
            }
        }

        public static void memClear() {
            var mem = new MemoryManagement();
            mem.FlushMemory();
            mem = null;
        }

        public static bool isProcessRunning(string sProcessName)
        {
            return ((Process.GetProcessesByName(sProcessName).Length > 0));
        }

        public static string versaoApp() {
            var sVersao = "";
            try {
                sVersao = string.Concat("Versão: v", Assembly.GetExecutingAssembly().GetName().Version.ToString());

                var nomeServidorAplicacao = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                var port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
                int pos = nomeServidorAplicacao.LastIndexOf(".");
                sVersao += pos >= 0
                               ? nomeServidorAplicacao.Substring(pos) + ":" + port
                               : nomeServidorAplicacao + ":" + port;
            } catch {
                if (sVersao != null) sVersao = ".srv_erro";
            }

            return sVersao;
        }

        public static bool IsNumeric(string data)
        {
            try {
                Convert.ToInt64(data);
                return true;
            } catch {
                return false;
            }
        }

        public static void IdInvalido(string _id, string callback) {
            if (string.IsNullOrEmpty(_id) || !IsNumeric(_id))
            {
                var json = "{\"error_message\": \"Identificador (" + _id + ") invalido!!!\" }";

                HttpContext.Current.Response.ContentType = (!string.IsNullOrEmpty(callback)) ? "application/javascript" : "text/html";
                HttpContext.Current.Response.Write(json);
                HttpContext.Current.Response.End();
                
             }
        }

        public static void FileValidaTamanho(string fileName, int tamanho, string callback)
        {
            if (tamanho > 41943040)
            {
                var json = "{\"error_message\": \"O arquivo "+fileName+" é maior que o limite suportado 40MB.\" }";
                HttpContext.Current.Response.ContentType = (!string.IsNullOrEmpty(callback)) ? "application/javascript" : "text/html";
                HttpContext.Current.Response.Write(json);
                HttpContext.Current.Response.End();
            }
        }

        public static void FileInvalido(string fileName,string extensions ,string callback)
        {
            var arrayExtencions = extensions.Split(',');
            bool isFilePermitted = false;
             foreach (var extension in arrayExtencions)
            {
                if (GetExtension(fileName) == extension.Replace(" ", "") && !string.IsNullOrEmpty(fileName))
                {
                    isFilePermitted = true;
                    break;
                }
                else if (string.IsNullOrEmpty(fileName))
                {
                    isFilePermitted = true;
                    break;
                }
            }
             if (!isFilePermitted)
             {

                 var IdNavegador = HttpContext.Current.Request.Browser.Id;
                 var json = "{\"error_message\": \"Arquivo não permitido\" }";

                HttpContext.Current.Response.ContentType = (!string.IsNullOrEmpty(callback)) ? "application/javascript" : "text/html";
                HttpContext.Current.Response.Write(json);
                HttpContext.Current.Response.End();
             }        
        }

        public static string GetExtension(string fileName)
        {
            var extension = "";
            try
            {
                extension = Path.GetExtension(fileName).ToLowerInvariant().Replace(".","");
            }
            catch
            {
                extension = fileName.Substring(fileName.LastIndexOf(".")).ToLowerInvariant();
            }
            return extension;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static bool Compare(string first, string second, bool bIgnoreCase, bool bIgnoreCaracteresSpecials){
            if (bIgnoreCaracteresSpecials)
            {
                first = RemoverCaracteresEspeciais(first);
                second = RemoverCaracteresEspeciais(second);
            }
            var iEqual = String.Compare(first, second, bIgnoreCase);
            return iEqual == 0;
        }

        public static string RemoverCaracteresEspeciais(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            texto = sb.ToString();
            string[] caracteresEspeciais = new string[] { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", " " };
            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                texto = texto.Replace(caracteresEspeciais[i], "");
            }
            return texto;
        }
        

     }
}
