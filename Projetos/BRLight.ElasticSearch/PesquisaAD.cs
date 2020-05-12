using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using util.BRLight;

namespace BRLight.ElasticSearch
{
    public class PesquisaAD<T> where T : new()
    {
        public FeedbackOV Salvar(T objeto, PesquisaES pesquisaEs)
        {
            string json = JSON.Serialize<T>(objeto);
            string result = Post(json, pesquisaEs.url + (pesquisaEs.Id != "" ? pesquisaEs.Id : ""));
            return JsonConvert.DeserializeObject<FeedbackOV>(result);
        }
        public PesquisaOV<T> Pesquisar(PesquisaES pesquisaEs)
        {
            string result = "";
            if (!string.IsNullOrEmpty(pesquisaEs.sQuery))
            {
                var fields = "";
                foreach(var field in pesquisaEs.fields)
                {
                    fields += (fields == "" ? "?fields=" + field : "," + field);
                }
                result = Post(pesquisaEs.sQuery, pesquisaEs.url + "/_search" + fields);
            }
            else
            {
                string parametros = "";
                if(pesquisaEs.size > 0)
                {
                    parametros = "?from=" + pesquisaEs.from + "&size=" + pesquisaEs.size;
                }
                result = Get(pesquisaEs.url + "/_search" + parametros);
            }
            //return JSON.Deserializa<PesquisaOV<T>>(result);
            return JsonConvert.DeserializeObject<PesquisaOV<T>>(result);
        }
        public Hit<T> Documento(PesquisaES pesquisaEs)
        {
            string result = Get(pesquisaEs.url + "/" + pesquisaEs.Id);
            return JsonConvert.DeserializeObject<Hit<T>>(result);
        }

        private string Post(string body, string uri)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(body);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                string stringResponse = reader.ReadToEnd();
                return stringResponse;
            }
            catch (Exception ex)
            {
                throw new ElasticSearchException("Erro na requisição do tipo POST. URI: " + uri, ex);
            }
        }
        private string Get(string uri)
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
                throw new ElasticSearchException("Erro na requisição do tipo GET. URI: " + uri, ex);
            }
        }
    }
}