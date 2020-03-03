using System.Collections.Generic;

namespace Exportador_LB_to_ES.AD.Models
{
    public class ParamsForIndexer
    {
        public ParamsForIndexer()
        {
            AuxiliarDeRankeamento = new List<string>();
        }
        public string TipoENumero { get; set; }
        public string Numero { get; set; }
        public string DataAssinatura { get; set; }
        public List<string> AuxiliarDeRankeamento { get; set; }
    }
}
