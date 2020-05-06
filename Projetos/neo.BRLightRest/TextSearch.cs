using System;
using System.Collections.Generic;
using util.BRLight;

namespace neo.BRLightREST
{
    public class TextSearch : neoBRLightREST
    {
        public string GetUriSearch(string uri)
        {
            return uri + "/_search";
        }
        public string GetUriMapping(string uri)
        {
            return uri + "/_mapping";
        }

        //public string Pesquisar(string elasticSearchUri, string[] camposPesquisar, string busca, string[] camposExibir)
        //{
        //    string query = CriarQuery(busca, camposPesquisar);
        //    string fields = "";
        //    for (var i = 0; i < camposExibir.Length; i++ )
        //    {
        //        fields = fields + (!string.IsNullOrEmpty(fields) ? ", " : "") + camposExibir[i];
        //    }
        //    if(fields != "")
        //    {
        //        fields = "?fields=" + fields;
        //    }
        //    Post(query, GetUriSearch(elasticSearchUri) + fields);
        //    return Response;
        //}

        //public string Pesquisar(string elasticSearchUri, string[] camposPesquisar, string busca)
        //{
        //    try
        //    {
        //        string query = CriarQuery(busca, camposPesquisar);
        //        Post(query, GetUriSearch(elasticSearchUri));
        //        return Response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FalhaOperacaoException("neoBRLightREST BuscaTextual: Não foi possível pesquisar \"" + busca + "\" em URI: " + iUri, ex);
        //    }
        //}

        //public string Pesquisar(string elasticSearchUri, string busca, string[] camposExibir)
        //{
        //    try
        //    {
        //        string query = CriarQuery(busca);
        //        string fields = "";
        //        for (var i = 0; i < camposExibir.Length; i++)
        //        {
        //            fields = fields + (!string.IsNullOrEmpty(fields) ? ", " : "") + camposExibir[i];
        //        }
        //        if (fields != "")
        //        {
        //            fields = "?fields=" + fields;
        //        }
        //        Post(query, GetUriSearch(elasticSearchUri) + fields);
        //        return Response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FalhaOperacaoException("neoBRLightREST BuscaTextual: Não foi possível pesquisar \"" + busca + "\" em URI: " + iUri, ex);
        //    }
        //}

        public string Pesquisar(string elasticSearchUri, string query)
        {
            try
            {
                Post(query, GetUriSearch(elasticSearchUri));
                return Response;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BuscaTextual: Não foi possível pesquisar \""+ query +"\" em URI: " + iUri, ex);
            }
        }

        public string Doc(string elasticSearchUri, string id_doc, string[] camposExibir)
        {
            string fields = "";
            for (var i = 0; i < camposExibir.Length; i++)
            {
                fields = fields + (!string.IsNullOrEmpty(fields) ? ", " : "") + camposExibir[i];
            }
            if (fields != "")
            {
                fields = "?fields=" + fields;
            }
            Get(elasticSearchUri + "/" + id_doc + fields);
            return Response;
        }

        public string Doc(string elasticSearchUri, string id_doc)
        {
            Get(elasticSearchUri + "/" + id_doc);
            return Response;
        }

        private void Post(string query, string uri)
        {
            REST objRest;
            objRest = new REST(uri, HttpVerb.POST, query) { RequestTimeOut = TimeOut };
            preencheResponse(ref objRest);
        }

        private void Get(string uri)
        {
            REST objRest;
            objRest = new REST(uri, HttpVerb.GET, "") { RequestTimeOut = TimeOut };
            preencheResponse(ref objRest);
        }

        public string CriarQuery(string busca)
        {
            Params.CheckNotNullOrEmpty("Busca", busca);
            return "{\"query\":{\"query_string\":{\"query\":(" + TrataTermosDaBusca(busca) + ")}}}";
        }

        public string CriarQuery(string busca, List<Sort> lSort)
        {
            Params.CheckNotNullOrEmpty("Busca", busca);
            string sSort = MontarSortString(lSort);
            return "{\"query\":{\"query_string\":{\"query\":(" + TrataTermosDaBusca(busca) + ")}} "+sSort+"}";
        }

        public string CriarQuery(string busca, int from, int size)
        {
            Params.CheckNotNullOrEmpty("Busca", busca);
            string range = ", \"from\":" + from + ", \"size\": " + size;
            return "{\"query\":{\"query_string\":{\"query\":(" + TrataTermosDaBusca(busca) + ")}}"+range+"}";
        }

        public string CriarQuery(string busca, int from, int size, List<Sort> lSort)
        {
            Params.CheckNotNullOrEmpty("Busca", busca);
            string range = ", \"from\":" + from + ", \"size\": " + size;
            string sSort = MontarSortString(lSort);
            return "{\"query\":{\"query_string\":{\"query\":\"(" + TrataTermosDaBusca(busca) + ")\"}} " + range + sSort + "}";
        }

        private string MontarSortString(List<Sort> lSort)
        {
            string sSort = "";
            foreach (var sort in lSort)
            {
                sSort = sSort + (sSort != "" ? "," : "") + sort.GetSort();
            }
            if (sSort != "")
            {
                sSort = ", \"sort\":[" + sSort + "]";
            }
            return sSort;
        }

        //private string CriarQuery(string busca, string[] camposPesquisar)
        //{
        //    Params.CheckNotNullOrEmpty("Busca", busca);
        //    Params.CheckIsEmptyArray("CamposPesquisar", camposPesquisar);
        //    string fields = "_all";
        //    for (var i = 0; i < camposPesquisar.Length; i++ )
        //    {
        //        fields = fields + (string.IsNullOrEmpty(fields) ? "" : ", ") + camposPesquisar[i];
        //    }
        //    return "{\"query\":{\"query_string\":{\"fields\":["+fields+"]\"query\":(" + TrataTermosDaBusca(busca) + ")}}}";
        //}

        private string TrataTermosDaBusca(string busca)
        {
            string[] palavrasDaBusca = busca.TrimStart(' ').TrimEnd(' ').Split(' ');
            string buscaTratada = "";
            foreach (var palavra in palavrasDaBusca)
            {
                buscaTratada = buscaTratada + TrataTermoDaBusca(palavra) + " ";
            }
            return buscaTratada.Remove(buscaTratada.Length - 1);
        }

        private string TrataTermoDaBusca(string termo)
        {
            return termo.Replace(".", "").Replace(",", "").Replace("-", "").Replace("\"", "\\\"").TrimStart('0');
        }
    }

    public class Sort
    {
        public Sort(string nm_field, string order)
        {
            _nm_field = nm_field;
            _order = order;
        }
        private string _nm_field;
        private string _order;
        public string GetSort()
        {
            return "{\"" + _nm_field + "\":{\"order\":\"" + _order + "\"}}";
        }
    }
}