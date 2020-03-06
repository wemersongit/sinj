namespace AcessaDadosElasticSearch.Objetos
{
	public class ObjetoConsultaElasticSearch<T>
	{
	    public int took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits<T> hits { get; set; }
	}
}

