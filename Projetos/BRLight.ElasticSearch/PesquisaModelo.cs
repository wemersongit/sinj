using System.Collections.Generic;

namespace BRLight.ElasticSearch
{   
    public enum Api
    {
        nothing,
        search,
        count,
        mapping
    }
    public enum Connector
    {
        and,
        or,
        not
    }
    public enum Order
    {
        desc,
        asc
    }
    public class Query
    {
        public Query Busca { get; set; }
        public Filtered BuscaFiltrada { get; set; }
        public Term Termo { get; set; }
        public QueryString BuscaTexto { get; set; }
        public Range Intervalo { get; set; }
    }
    public class Term
    {
        public string Campo { get; set; }
        public string Valor { get; set; }
    }

    public class QueryFilter
    {
        public Connector Conector { get; set; }
        public Query Busca { get; set; }
    }

    public class Filter
    {
        public Filter()
        {
            Filters = new List<QueryFilter>();
        }

        public List<QueryFilter> Filters { get; set; }
    }
    public class Filtered
    {
        public Filter Filtro { get; set; }
    }
    public class QueryString
    {
        public QueryString()
        {
            Campos = new List<string>();
            DefaultOperador = Connector.or;
        }
        public string Valor { get; set; }
        public List<string> Campos { get; set; }
        public Connector DefaultOperador { get; set; }

    }
    public class Range
    {
        public string Campo { get; set; }
        public bool OuIgual { get; set; }
        public string De { get; set; }
        public string Ate { get; set; }
    }
    public class Sort
    {
        public Sort()
        {
            Ordem = Order.asc;
        }
        public string Campo { get; set; }
        public Order Ordem { get; set; }
    }

    public class FeedbackOV
    {
        public bool ok { get; set; }
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public int _version { get; set; }
    }

    public class PesquisaOV<T>
    {
        public PesquisaOV()
        {
            _shards = new Shards();
            hits = new Hits<T>();
        }
        public int took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits<T> hits { get; set; }
    }

    public class Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
    }

    public class Hits<T>
    {
        public Hits()
        {
            hits = new List<Hit<T>>();
        }
        public ulong total { get; set; }
        public double? max_score { get; set; }
        public List<Hit<T>> hits { get; set; }
    }

    public class Hit<T>
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public double? _score { get; set; }
        public int _version { get; set; }
        public bool found { get; set; }
        public T _source { get; set; }
        public T fields { get; set; }
    }

}