using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class ArquivoVersionadoNormaOV : ArquivoVersionado
    {
        public string ch_norma { get; set; }
        public string ch_vide { get; set; }
        public NormaOV norma { get; set; }
    }
}
