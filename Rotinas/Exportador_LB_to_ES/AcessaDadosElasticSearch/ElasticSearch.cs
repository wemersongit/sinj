using System;
using System.IO;
using System.Net;
using System.Text;

namespace AcessaDadosElasticSearch
{
    public class ElasticSearch
    {
        /// <summary>
        /// Envia um registro via requisição do tipo POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        public void Post(string url, string json)
        {
            try
            {
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                //request.Credentials = new NetworkCredential("brlight", "brlight");
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                reader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível enviar registro via post", ex);
            }
        }

        public string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                string stringResponse = reader.ReadToEnd();
                return stringResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar requisicao GET", ex);
            }
        }

        public string Delete(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "DELETE";
                request.Timeout = 15000;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                string stringResponse = reader.ReadToEnd();
                return stringResponse;

            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possível deletar registro via DELETE", ex);
            }
        }
    }
}
