using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class InteressadoOV : metadata
    {
        public InteressadoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_interessado { get; set; }
        public string nm_interessado { get; set; }
        public string ds_interessado { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Interessado
    {
        public string ch_interessado { get; set; }
        public string nm_interessado { get; set; }
    }
}
