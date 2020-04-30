using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;
using neo.BRLightREST;

namespace neo.BRLightES
{
    internal class AD
    {
        internal string IncluirDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.POST, json_doc).GetResponse();
        }
        internal string AtualizarDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.PUT, json_doc).GetResponse();
        }
        internal string DeletarDoc(string json_doc, string uri)
        {
            return new REST(uri, HttpVerb.DELETE, json_doc).GetResponse();
        }
        internal Result<T> PesquisarDocs<T>(string json_consulta, string uri)
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
                throw new Exception("Erro na requisição do tipo POST. URI: " + uri, ex);
            }
        }

        internal string PesquisarTotal(string json_consulta, string uri)
        {
            try
            {
                return new REST(uri, HttpVerb.POST, json_consulta).GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na requisição do tipo POST. URI: " + uri, ex);
            }
        }

        internal string GetUrlEs(string nm_base)
        {
            return new Base().GetPathBase(nm_base, "metadata/idx_exp_url");
        }
    }
}
