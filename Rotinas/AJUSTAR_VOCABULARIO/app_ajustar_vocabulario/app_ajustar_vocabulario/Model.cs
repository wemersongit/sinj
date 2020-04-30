using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app_ajustar_vocabulario
{
    public class LegadoNormaLBW
    {
        public LegadoNormaLBW()
        {
            neoindexacao = new List<NeoIndexacao>();
        }
        public string id { get; set; }
        public List<NeoIndexacao> neoindexacao { get; set; }
    }

    public class NeoIndexacao
    {
        public int intipotermo;
        public string nmtermo;
        public string nmespecificador;
    }
}
