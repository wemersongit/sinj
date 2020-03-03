using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class ArquivoFullOV : ArquivoOV
    {
        public ulong id_doc { get; set; }
        public string filetext { get; set; }
    }
}
