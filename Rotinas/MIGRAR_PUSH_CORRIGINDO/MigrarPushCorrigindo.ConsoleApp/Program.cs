using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using System.IO;
using util.BRLight;
using System.Net;

namespace MigrarPushCorrigindo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OrgaoOV orgaoOv;
            TipoDeNormaOV tipo_de_normaOv;
            VocabularioOV vocabularioOv;
            var notifiquemeRn = new NotifiquemeRN(false);
            var tipo_de_normaRn = new TipoDeNormaRN();
            var orgaoRn = new OrgaoRN();
            var vocabularioRn = new VocabularioRN();
            var lista_notifiqueme = notifiquemeRn.Consultar(new Pesquisa() { limit = null });

            foreach (var usuario_notifiqueme in lista_notifiqueme.results)
            {
                var j = 0;
                foreach (var parametros_criacao_norma in usuario_notifiqueme.criacao_normas_monitoradas)
                {
                    parametros_criacao_norma.ch_criacao_norma_monitorada = Guid.NewGuid().ToString("N");
                    // Se algum parametro for 0, é um valor inválido. Será convertido para null
                    if (parametros_criacao_norma.ch_orgao_criacao == "0")
                    {
                        parametros_criacao_norma.ch_orgao_criacao = "";
                    }
                    if (parametros_criacao_norma.ch_termo_criacao == "0")
                    {
                        parametros_criacao_norma.ch_termo_criacao = "";
                    }
                    if (parametros_criacao_norma.ch_tipo_norma_criacao == "0")
                    {
                        parametros_criacao_norma.ch_tipo_norma_criacao = "";
                    }
                    if (parametros_criacao_norma.ch_tipo_termo_criacao == "0")
                    {
                        parametros_criacao_norma.ch_tipo_termo_criacao = "";
                    }
                    // Se houver o primeiro conector mas não houver valor nenhum para ser conectado,
                    // o primeiro conector é transformado em vazio.
                    if (!string.IsNullOrEmpty(parametros_criacao_norma.primeiro_conector_criacao))
                    {
                        if (string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_termo_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
                        {
                            parametros_criacao_norma.primeiro_conector_criacao = "";
                        }
                    }
                    // Se o conector for null e houver valores que devem ser conectados na query de busca, 
                    // o conector é transformado em 'E'.
                    if (string.IsNullOrEmpty(parametros_criacao_norma.primeiro_conector_criacao))
                    {
                        if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_norma_criacao))
                        {
                            if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) || !string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
                            {
                                parametros_criacao_norma.primeiro_conector_criacao = "E";
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(parametros_criacao_norma.segundo_conector_criacao))
                    {
                        if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && !string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao))
                        {
                            parametros_criacao_norma.segundo_conector_criacao = "E";
                        }
                    }

                    if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_tipo_norma_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_tipo_norma_criacao))
                    {
                        tipo_de_normaOv = tipo_de_normaRn.Doc(parametros_criacao_norma.ch_tipo_norma_criacao);
                        parametros_criacao_norma.nm_tipo_norma_criacao = tipo_de_normaOv.nm_tipo_norma;
                    }
                    if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_orgao_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_orgao_criacao))
                    {
                        try
                        {
                            orgaoOv = orgaoRn.Doc(parametros_criacao_norma.ch_orgao_criacao);
                            parametros_criacao_norma.nm_orgao_criacao = orgaoOv.nm_orgao;
                        }
                        catch
                        {
                            parametros_criacao_norma.ch_orgao_criacao = "";
                        }
                    }

                    if (!string.IsNullOrEmpty(parametros_criacao_norma.ch_termo_criacao) && string.IsNullOrEmpty(parametros_criacao_norma.nm_termo_criacao))
                    {
                        vocabularioOv = vocabularioRn.Doc(parametros_criacao_norma.ch_termo_criacao);
                        parametros_criacao_norma.nm_termo_criacao = vocabularioOv.nm_termo;
                    }
                }
                var dic = new Dictionary<string, object> { { "value", usuario_notifiqueme } };
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

                string contentType = "multipart/form-data; boundary=" + formDataBoundary;
                byte[] formData = GetMultipartFormData(dic, formDataBoundary);

                using (var httpWebResponse = PostForm(Config.ValorChave("") + "/" + _nm_base + "/doc", contentType, formData))
                {
                    using (Stream streamResponse = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(streamResponse))
                        {
                            string stringResponse = reader.ReadToEnd();
                            reader.Close();
                        }
                        streamResponse.Dispose();
                    }
                    httpWebResponse.Close();
                }
                new AcessoAD<NotifiquemeOV>("push").Incluir(usuario_notifiqueme);
            }
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
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
                        formDataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, Encoding.UTF8.GetByteCount("\r\n"));

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

                        formDataStream.Write(Encoding.UTF8.GetBytes(header), 0, Encoding.UTF8.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                var footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();
            }
            return formData;
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
            //if (RequestTimeOut > 0) request.Timeout = RequestTimeOut;

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

        private HttpWebResponse GetResponseNoException(WebRequest WebRequest)
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
    }
}
