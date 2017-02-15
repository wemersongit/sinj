using System.Collections.Generic;

namespace BRLight.ElasticSearch
{
    public class PesquisaES
    {
        public PesquisaES()
        {
            //_query = new Query();
            sort = new List<Sort>();
            fields = new List<string>();
        }
        public string Id { get; set; }
        public string host { private get; set; }
        public string port { private get; set; }
        public string index { private get; set; }
        public string type { private get; set; }
        public Query query { private get; set; }
        private string _sQuery;
        public string sQuery
        {
            get
            {
                if(_sQuery == "")
                {
                    _sQuery = GetQuery(query);
                }
                return _sQuery;
            }
            set { _sQuery = value; }
        }
        public int from { get; set; }
        public int size { get; set; }
        public bool OrdenarRankeado { get; set; }
        public List<Sort> sort { private get; set; }
        private string _url;
        public string url
        {
            get
            {
                if(_url == "")
                {
                    _url = "http://" + host + ":" + port + "/" + index + "/" + type;
                }
                return _url;
            }
            set { _url = value; }
        }

        public List<string> fields { get; set; }

        private string GetQuery(){
            return query != null ? "{\"query\":" + GetQuery(query) + "" + (from > 0 ? ",\"from\":" + from + "" : "") + (size > 0 ? ",\"size\":" + size + "" : "") + (sort.Count > 0 ? "," + GetSort(sort, OrdenarRankeado) : "") + "}" : "";
        }
        private string GetSort(List<Sort> lSort, bool ordenarRankeado)
        {
            string sSort = "";
            foreach (var oSort in lSort)
            {
                sSort += (sSort != "" ? "," : "") + "{\"" + oSort.Campo + "\":{\"order\":\""+oSort.Ordem+"\"}}";
            }
            return "\"sort\":["+( ordenarRankeado ? "\"_score\"," : "" ) + sSort +"]";
        }
        private string GetQuery(Query oQuery)
        {
            string sQuery = "";
            if (oQuery != null)
            {
                if (oQuery.BuscaFiltrada != null)
                {
                    if (oQuery.BuscaFiltrada.Filtro != null)
                    {
                        sQuery += "{\"filtered\":" + GetFilter(oQuery.BuscaFiltrada.Filtro) +"}}";
                    }
                }
                else if(oQuery.Termo != null)
                {
                    sQuery += "{\"term\":{\"" + oQuery.Termo.Campo + "\":{\"value\":\"" + oQuery.Termo.Valor +"\"}}}";
                }
                else if(oQuery.Busca != null)
                {
                    sQuery += "{\"query\":"+GetQuery(oQuery.Busca)+"}";
                }
                else if(oQuery.BuscaTexto != null)
                {
                    sQuery += "{\"query\":"+GetQuery_string(oQuery.BuscaTexto)+"}";
                }
            }
            return sQuery;
        }
        private string GetQuery_string(QueryString oQueryString)
        {
            string sQueryString = "";
            if(oQueryString != null)
            {
                string campos = "";
                foreach (var campo in oQueryString.Campos)
                {
                    campos += (campos != "" ? "," : "") + "\"" + campo + "\"";
                }
                string operador = "";
                switch (oQueryString.DefaultOperador)
                {
                    case Connector.and:
                        operador = "\"AND\"";
                        break;
                    case Connector.not:
                        operador = "\"NOT\"";
                        break;
                    default:
                        operador = "\"OR\"";
                        break;

                }
                sQueryString += "{\"query_string\":{\"query\":\"(" + oQueryString.Valor + ")\", \"default_operator\":" + operador + (campos != "" ? ", \"fields\":[" + campos + "]" : "") +"}}";
            }
            return sQueryString;
        }
        private string GetFilter(Filter oFilter)
        {
            string filter = "";
            if(oFilter != null)
            {
                string query_and = "";
                string query_or = "";
                string query_not = "";
                foreach (var queryFilter in oFilter.Filters)
                {
                    switch (queryFilter.Conector)
                    {
                        case Connector.or:
                            query_or += (query_or != "" ? "," : "") + GetQuery(queryFilter.Busca);
                            break;
                        case Connector.not:
                            query_not += (query_not != "" ? "," : "") + GetQuery(queryFilter.Busca);
                            break;
                        default:
                            query_and += (query_and != "" ? "," : "") + GetQuery(queryFilter.Busca);
                            break;

                    }
                }
                filter = "{\"and\":[" + query_and + "], \"or\":["+query_or+"], \"not\":["+query_not+"] }";
            }
            return filter;
        }
    }
}