using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class TipoDeEdicaoOV : metadata
    {
        public TipoDeEdicaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }

        public string ch_tipo_edicao { get; set; }
        public string nm_tipo_edicao { get; set; }
		public string ds_tipo_edicao { get; set; }
        public bool st_edicao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        
    }
}
