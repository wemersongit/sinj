using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace util.BRLight
{
    public enum HttpVerb
    {
        [ValorString("GET", "1")]
        GET = 1,
        [ValorString("POST", "2")]
        POST = 2,
        [ValorString("PUT", "3")]
        PUT = 3,
        [ValorString("DELETE", "4")]
        DELETE = 4
    }
    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }

    public class REST : IDisposable
    {

        private readonly Encoding Encoding = Encoding.UTF8;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        private string _userAgent;
        private string statusCode;
        private string headers;
        private string statusDescription;
        private Dictionary<string, string> dictHeader;

        private bool _bAddCaracterForm { get; set; }

        public int RequestTimeOut { get; set; }

        private byte[] StreamToByte { get; set; }

        public string userAgent
        {
            get { return (string.IsNullOrEmpty(_userAgent)) ? "neo.BrLightBase" : _userAgent; }
            set { _userAgent = value; }
        }

        public void AddHeader(string name, string val)
        {
            request.Headers.Add(name, val);
        }

        public REST(string url, HttpVerb Method, Dictionary<string, object> postParameters)
        {
            var caseMethod = ValorString.PegarValorString(Method);
            switch (caseMethod)
            {
                case "GET":
                    request = GetForm(url);
                    break;
                case "PUT":
                case "DELETE":
                case "POST":
                    request = MultipartFormDataPost(url, postParameters, Method);
                    break;
                default:
                    throw new Exception("REST: invalid Method Type");
            }
        }

        public REST(string url, HttpVerb Method, Dictionary<string, object> postParameters, bool bAddCaracterForm)
        {
            _bAddCaracterForm = bAddCaracterForm;
            var caseMethod = ValorString.PegarValorString(Method);
            switch (caseMethod)
            {
                case "GET":
                    request = GetForm(url);
                    break;
                case "PUT":
                case "DELETE":
                case "POST":
                    request = MultipartFormDataPost(url, postParameters, Method);
                    break;
                default:
                    throw new Exception("REST: invalid Method Type");
            }
        }

        public REST(string url, HttpVerb Method, string postJsonParameters)
        {
            var caseMethod = ValorString.PegarValorString(Method);
            switch (caseMethod)
            {
                case "GET":
                    request = GetForm(url);
                    break;
                case "PUT":
                case "DELETE":
                case "POST":
                    request = JsonDataPost(url, postJsonParameters, Method);
                    break;
                default:
                    throw new Exception("REST: invalid Method Type");
            }
        }

        ~REST()
        {
            Dispose();
        }

        public void Dispose()
        {
            StreamToByte = null;
            //  request = null;
        }

        public HttpWebRequest GetForm(string url)
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            request = (HttpWebRequest)WebRequest.Create(url);
            if (request == null)
            {
                throw new NullReferenceException("REST: Não foi possivel criar um HttpWebRequest para: " + url);
            }
            // request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;
            request.UserAgent = userAgent;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "GET";

            if (RequestTimeOut > 0) request.Timeout = RequestTimeOut;

            return request;
        }

        public HttpWebRequest GetJson(string url)
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            request = (HttpWebRequest)WebRequest.Create(url);
            if (request == null)
            {
                throw new NullReferenceException("REST: Não foi possivel criar um HttpWebRequest para: " + url);
            }
            // request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;
            request.UserAgent = userAgent;
            request.ContentType = "application/json";
            request.Method = "GET";

            if (RequestTimeOut > 0) request.Timeout = RequestTimeOut;

            return request;
        }

        private HttpWebRequest MultipartFormDataPost(string postUrl, Dictionary<string, object> postParameters, HttpVerb Method)
        {
            string formDataBoundary;
            if (_bAddCaracterForm)
            {
                formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid() + "$");
            }
            else
            {
                formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            }

            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return PostForm(postUrl, contentType, formData, Method);
        }

        private HttpWebRequest JsonDataPost(string postUrl, string postJsonParameters, HttpVerb Method)
        {
            byte[] formData = Encoding.UTF8.GetBytes(postJsonParameters);
            string contentType = "application/json;";
            return PostForm(postUrl, contentType, formData, Method);
        }

        private HttpWebRequest PostForm(string postUrl, string contentType, byte[] formData, HttpVerb Method)
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("REST: Não foi possivel criar um HttpWebRequest para: " + postUrl);
            }

            // Set up the request properties.
            // request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;
            request.Method = ValorString.PegarValorString(Method);
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            if (RequestTimeOut > 0) request.Timeout = RequestTimeOut;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request;
        }

        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new MemoryStream();
            var needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(Encoding.GetBytes("\r\n"), 0, Encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    var fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(Encoding.GetBytes(postData), 0, Encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            var footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.GetBytes(footer), 0, Encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            var formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public string GetStatusCode()
        {
            return statusCode;
        }

        public string GetStatusDescription()
        {
            return statusDescription;
        }

        public Dictionary<string, string> GetDictHeader()
        {
            return dictHeader;
        }

        public string GetHeaders()
        {
            return headers;
        }


        public byte[] GetResponseStream()
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Credentials = CredentialCache.DefaultCredentials;
                using (HttpWebResponse response1 = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    response = response1;
                    var m = new MemoryStream();


                    headers = "Status code: " + GetStatusCode() + "\r\n";
                    headers += "Status description: " + GetStatusDescription() + "\r\n";
                    foreach (string key in response.Headers.Keys)
                    {
                        headers += string.Format("{0}: {1} \r\n", key, response.Headers[key]);
                    }

                    dictHeader = new Dictionary<string, string>();
                    foreach (string key in response.Headers.Keys)
                    {
                        dictHeader.Add(key, response.Headers[key]);
                    }

                    statusCode = response.StatusCode.ToString();

                    statusDescription = response.StatusDescription;
                    try
                    {
                        response.GetResponseStream().CopyTo(m);
                        StreamToByte = m.ToArray();
                    }
                    finally
                    {
                        m.Close();
                    }
                    return StreamToByte;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public string GetResponse()
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.Credentials = CredentialCache.DefaultCredentials;
                using (HttpWebResponse response1 = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    response = response1;
                    string target;

                    var m = new MemoryStream();
                    response.GetResponseStream().CopyTo(m);
                    StreamToByte = m.ToArray();



                    headers = "Status code: " + GetStatusCode() + "\r\n";
                    headers += "Status description: " + GetStatusDescription() + "\r\n";
                    foreach (string key in response.Headers.Keys)
                    {
                        headers += string.Format("{0}: {1} \r\n", key, response.Headers[key]);
                    }

                    dictHeader = new Dictionary<string, string>();
                    foreach (string key in response.Headers.Keys)
                    {
                        dictHeader.Add(key, response.Headers[key]);
                    }

                    statusCode = response.StatusCode.ToString();

                    statusDescription = response.StatusDescription;
                    try
                    {
                        target = Encoding.GetString(StreamToByte);
                    }
                    finally
                    {
                        m.Close();
                    }

                    return target;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }

}
