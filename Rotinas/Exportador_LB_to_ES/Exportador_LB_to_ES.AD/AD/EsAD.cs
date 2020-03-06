using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using AcessaDadosElasticSearch;
using System.Text;
using Exportador_LB_to_ES.util;

namespace Exportador_LB_to_ES.AD.AD
{
    public class EsAD
    {
        private ElasticSearch _elasticSearch;

        public EsAD()
        {
            _elasticSearch = new ElasticSearch();
        }

        public static bool ValidaSerializacao(object objeto)
        {
            try
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                StringBuilder stringBuilder = new StringBuilder();
                jss.Serialize(objeto, stringBuilder);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Não é possível serializar objeto", ex);
            }
        }

        public List<string> IndexarNoElasticSearch<T>(string uri, string type, List<T> lista, string propId)
        {
            List<string> idsRegistrosIndexados = new List<string>();
            foreach (object termo in lista)
            {
                string id = Util.GetPropValue(termo, propId).ToString();
                try
                {
                    string json = JSON.Serializa(termo);
                    _elasticSearch.Post(string.Format("{0}{1}/{2}", uri, type, id), json);
                    idsRegistrosIndexados.Add(id);
                    Console.WriteLine(" >>>> " + type + " indexado: " + id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao indexar " + type + " " + id);
                    Log.LogarExcecao("Exportação de " + type, "Erro ao indexar " + type + " " + id, ex);
                }
            }
            return idsRegistrosIndexados;
        }

        public void ConfigurarIndexType(string urlIndexacaoElasticSearch, string mapping, string type)
        {
            try
            {
                Console.WriteLine(urlIndexacaoElasticSearch + type + "/_mapping");
                _elasticSearch.Post(urlIndexacaoElasticSearch + type + "/_mapping", mapping);
            }
            catch (Exception ex)
            {
                Log.LogarExcecao("Criando Mapping", "Nao foi possivel configurar o mapping de " + type, ex);
            }
        }

        public bool DelatarNoElasticSearch(string uri)
        {
            Console.WriteLine("Deletando " + uri);
            bool deletado = false;
            try
            {
                _elasticSearch.Delete(uri);
                deletado = true;
            }
            catch(Exception ex)
            {
                Console.Write(" - ERRO");
                Log.LogarExcecao("Deletando no ES", "Erro ao deletar " + uri, ex);
            }
            return deletado;
        }
    }
}
