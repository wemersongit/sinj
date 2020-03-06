using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;
using System.Net;
using System.IO;

namespace MigradorSINJ
{
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
    public class REST
    {
        private readonly Encoding _encoding = Encoding.UTF8; 
        private string _nm_base;
        private string _url;
        private int RequestTimeOut { get; set; }

        public REST(string nm_base)
        {
            _nm_base = nm_base;
            _url = Config.ValorChave("url_rest",true);
        }

        public string Consultar(string consulta)
        {
            string stringResponse = "";
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            using (var httpWebResponse = Get(_url + "/" + _nm_base + "/doc?" + consulta, contentType))
            {
                using (Stream streamResponse = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(streamResponse))
                    {
                        stringResponse = reader.ReadToEnd();
                        reader.Close();
                    }
                    streamResponse.Dispose();
                }
                httpWebResponse.Close();
            }
            return stringResponse;
        }

        public ulong Incluir(object ov)
        {
            ulong id = 0;
            var json = JSON.Serialize(ov);
            var dic = new Dictionary<string, object> { { "value", json } };
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(dic, formDataBoundary);

            using (var httpWebResponse = PostForm(_url + "/" + _nm_base + "/doc", contentType, formData))
            {
                using (Stream streamResponse = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(streamResponse))
                    {
                        string stringResponse = reader.ReadToEnd();
                        try
                        {
                            id = Convert.ToUInt64(stringResponse);
                        }
                        catch
                        {
                            id = 0;
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
                            sb.AppendLine("Retorno do REST: " + stringResponse);
                            sb.AppendLine("JsonDoc: " + json);
                            EscreverLogDocNaoMigrado(sb.ToString(), Guid.NewGuid().ToString());
                        }
                        reader.Close();
                    }
                    streamResponse.Dispose();
                }
                httpWebResponse.Close();
            }
            return id;
        }
        public bool Atualizar<T>(ulong id_doc, T ov)
        {
            var updated = false;
            var json = JSON.Serialize(ov);
            var dic = new Dictionary<string, object> { { "value", json } };
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(dic, formDataBoundary);

            using (var httpWebResponse = PostForm(_url + "/" + _nm_base + "/doc/" + id_doc, contentType, formData, "PUT"))
            {
                using (Stream streamResponse = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(streamResponse))
                    {
                        var supdated = reader.ReadToEnd();
                        updated = supdated.ToUpper() == "UPDATED";
                        if (!updated)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
                            sb.AppendLine("Retorno do REST: " + updated);
                            sb.AppendLine("JsonDoc: " + json);
                            EscreverLogDocNaoMigrado(sb.ToString(), id_doc.ToString());
                        }
                        reader.Close();
                    }
                    streamResponse.Dispose();
                }
                httpWebResponse.Close();
            }
            return updated;
        }
        public bool AtualizarPath(ulong id_doc, string caminho, string valor, string retorno)
        {
            var updated = false;
            var dic = new Dictionary<string, object> { { "value", valor } };
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(dic, formDataBoundary);

            using (var httpWebResponse = PostForm(_url + "/" + _nm_base + "/doc/" + id_doc + "/"+caminho, contentType, formData, "PUT"))
            {
                using (Stream streamResponse = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(streamResponse))
                    {
                        var supdated = reader.ReadToEnd();
                        updated = supdated.ToUpper() == "UPDATED";
                        if (!updated)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("=========" + DateTime.Now.ToString() + "=========");
                            sb.AppendLine("Retorno do REST: " + updated);
                            sb.AppendLine("Path: " + caminho);
                            sb.AppendLine("valuePath: " + valor);
                            EscreverLogDocNaoMigrado(sb.ToString(), id_doc.ToString());
                        }
                        reader.Close();
                    }
                    streamResponse.Dispose();
                }
                httpWebResponse.Close();
            }
            return updated;
        }

        public byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            var formData = new byte[0];
            using (Stream formDataStream = new MemoryStream())
            {
                var needsCLRF = false;

                foreach (var param in postParameters)
                {
                    // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsCLRF)
                        formDataStream.Write(_encoding.GetBytes("\r\n"), 0, _encoding.GetByteCount("\r\n"));

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

                        formDataStream.Write(_encoding.GetBytes(header), 0, _encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(_encoding.GetBytes(postData), 0, _encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                var footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(_encoding.GetBytes(footer), 0, _encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();
            }
            return formData;
        }

        public HttpWebResponse Get(string postUrl, string contentType)
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("REST: Não foi possivel criar um HttpWebRequest para: " + postUrl);
            }

            request.Proxy = null;
            request.Method = "GET";
            request.ContentType = contentType;
            request.UserAgent = "neo.BrLightBase";
            request.CookieContainer = new CookieContainer();
            if (RequestTimeOut > 0) request.Timeout = RequestTimeOut;

            return GetResponseNoException(request);
        }

        public HttpWebResponse PostForm(string postUrl, string contentType, byte[] formData, string method = "POST")
        {
            System.Net.WebRequest.DefaultWebProxy = null;
            ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("REST: Não foi possivel criar um HttpWebRequest para: " + postUrl);
            }

            // Set up the request properties.
            // request.ServicePoint.Expect100Continue = false;
            request.Proxy = null;
            request.Method = method;
            request.ContentType = contentType;
            request.UserAgent = "neo.BrLightBase";
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

            return GetResponseNoException(request);
        }

        private static HttpWebResponse GetResponseNoException(WebRequest WebRequest)
        {
            try
            {
                return (HttpWebResponse)WebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                var resp = (HttpWebResponse)ex.Response;
                if (resp == null)
                    throw new Exception("REST: GetResponseNoException: ", ex);
                return resp;
            }
        }
        private void EscreverLogDocNaoMigrado(string log, string nm)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
            }
            StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log\erro_rest_"+_nm_base+"_"+nm+".txt", true);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }
    }

    public class Doc
    {
        private REST _rest;
        private string _url;
        private string _nm_base;
        public Doc(string nm_base)
        {
            _rest = new REST(nm_base);
            _nm_base = nm_base;
            _url = Config.ValorChave("url_rest", true);
        }

        public string incluir(Dictionary<string, object> parametros)
        {
            string chaveDoc;
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = _rest.GetMultipartFormData(parametros, formDataBoundary);

            using (var httpWebResponse = _rest.PostForm(_url + "/" + _nm_base + "/file", contentType, formData))
            {
                using (Stream streamResponse = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(streamResponse))
                    {
                        chaveDoc = reader.ReadToEnd();
                        reader.Close();
                    }
                    streamResponse.Dispose();
                }
                httpWebResponse.Close();
            }
            return chaveDoc;
        }
    }
}
