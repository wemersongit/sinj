using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class RelatorOV : metadata
    {
        public RelatorOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_relator { get; set; }
        public string nm_relator { get; set; }
        public string ds_relator { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Relator
    {
        public string ch_relator { get; set; }
        public string nm_relator { get; set; }
    }
}
