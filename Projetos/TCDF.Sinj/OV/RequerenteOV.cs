using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class RequerenteOV : metadata
    {
        public RequerenteOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_requerente { get; set; }
        public string nm_requerente { get; set; }
        public string ds_requerente { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Requerente
    {
        public string ch_requerente { get; set; }
        public string nm_requerente { get; set; }
    }
}
