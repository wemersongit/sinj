using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class ProcuradorOV : metadata
    {
        public ProcuradorOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_procurador { get; set; }
        public string nm_procurador { get; set; }
        public string ds_procurador { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Procurador
    {
        public string ch_procurador_responsavel { get; set; }
        public string nm_procurador_responsavel { get; set; }
    }
}
