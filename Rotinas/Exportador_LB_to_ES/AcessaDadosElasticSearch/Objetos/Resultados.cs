namespace AcessaDadosElasticSearch.Objetos
{
	public class Resultados<T>
	{
		public static int num = 0;
		public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public double? _score { get; set; }
        public T _source { get; set; }
        public T fields { get; set; }

        
	}
}
