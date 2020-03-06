using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class TipoDeFonteOV : metadata
    {
        public TipoDeFonteOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }

        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
		public string ds_tipo_fonte { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        
    }
}
