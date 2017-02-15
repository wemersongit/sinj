using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class TipoDePublicacaoOV : metadata
    {
        public TipoDePublicacaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_tipo_publicacao { get; set; }
        public string nm_tipo_publicacao { get; set; }
        public string ds_tipo_publicacao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

    }
}