using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class ArquivoVersionado
    {
        public string ch_arquivo_versionado { get; set; }
        public ArquivoOV ar_arquivo_versionado { get; set; }
        public string dt_arquivo_versionado { get; set; }
        public string nm_login_usuario { get; set; }
    }
}
