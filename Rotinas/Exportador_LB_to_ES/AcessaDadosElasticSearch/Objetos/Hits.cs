using System;
using System.Collections.Generic;

namespace AcessaDadosElasticSearch.Objetos
{
	public class Hits<T>
	{
		public int total { get; set; }

		public Double? max_score { get; set; }

		public List<Resultados<T>> hits { get; set; }
		
		public string LastId ()
		{
			return null;
		}
	}
}