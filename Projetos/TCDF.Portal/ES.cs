using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using TCDF.Portal.OV;
using util.BRLight;

namespace TCDF.Portal
{
    public class ES
    {
        public string IncluirDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.POST, json_doc).GetResponse();
        }
        public string AtualizarDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.PUT, json_doc).GetResponse();
        }
        public string DeletarDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.DELETE, json_doc).GetResponse();
        }
        public Result<T> PesquisarDocs<T>(string json_consulta, string uri)
        {
            try
            {
                Result<T> result = new Result<T>();
                string stringResponse = new REST(uri, HttpVerb.POST, json_consulta).GetResponse();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(stringResponse);
                return result;
            }
            catch (Exception ex)
            {
                throw new ESException("Erro na requisição do tipo POST. URI: " + uri, ex);
            }
        }
    }
}
