using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class SituacaoOV : metadata
    {
        public SituacaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_situacao { get; set; }
        public string nm_situacao { get; set; }
        public string ds_situacao { get; set; }
        public int nr_peso_situacao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

    }
}