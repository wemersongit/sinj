using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class SINJ_ArquivoOV : metadata
    {

        public SINJ_ArquivoOV()
        {
            alteracoes = new List<AlteracaoOV>();
            ar_arquivo = new ArquivoOV();
        }

        public string ch_arquivo { get; set; }
        public string nm_arquivo { get; set; }
        public string ds_arquivo { get; set; }
        public string ch_arquivo_superior { get; set; }
        public int nr_tipo_arquivo { get; set; }
        public int nr_nivel_arquivo { get; set; }
        public ArquivoOV ar_arquivo { get; set; }


        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}
