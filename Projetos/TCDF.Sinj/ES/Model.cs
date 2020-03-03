using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Sinj.ES
{
    public class Result<T>
    {
        public ulong took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits<T> hits { get; set; }
        public Dictionary<string, object> aggregations { get; set; }
        public Dictionary<string, ResultFacet> facets { get; set; }
    }

    public class ResultFacet
    {
        public string _type { get; set; }
        public ulong count { get; set; }
    }

    public class Aggregation
    {
        public AggTerms agg_terms { get; set; }
        public AggTpPesquisa agg_tipo_pesquisa { get; set; }
    }

    public class AggTerms
    {
        public ulong doc_count_error_upper_bound { get; set; }
        public ulong sum_other_doc_count { get; set; }
        public Bucket[] buckets { get; set; }
    }

    public class AggTpPesquisa
    {
        public BucketTpPesquisa buckets { get; set; }
        public BucketTpPesquisa norma { get; set; }
        public BucketTpPesquisa diario { get; set; }
        public BucketTpPesquisa avancada { get; set; }
    }

    public class Bucket
    {
        public string key { get; set; }
        public ulong doc_count { get; set; }
    }

    public class BucketTpPesquisa
    {
        public Bucket geral { get; set; }
        public Bucket norma { get; set; }
        public Bucket diario { get; set; }
        public Bucket avancada { get; set; }
    }

    public class Shards
    {
        public ulong? total { get; set; }
        public ulong? successful { get; set; }
        public ulong? failed { get; set; }
    }

    public class Hits<T>
    {
        public ulong total { get; set; }
        public Double? max_score { get; set; }
        public List<Resultados<T>> hits { get; set; }
        public string LastId()
        {
            return null;
        }
    }

    public class Resultados<T>
    {
        public static ulong num = 0;
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public string _score { get; set; }
        public T _source { get; set; }
        public Fields<T> fields { get; set; }
        public object highlight { get; set; }

    }

    public class Fields<T>
    {
        public T[] partial { get; set; }
    }

    public class ScanAndScroll<T>
    {
        public ScanAndScroll()
        {
            _shards = new object();
            hits = new Hits<T>();
        }
        public string _scroll_id { get; set; }
        public ulong took { get; set; }
        public bool timed_out { get; set; }
        public object _shards { get; set; }
        public Hits<T> hits { get; set; }
    }
}
