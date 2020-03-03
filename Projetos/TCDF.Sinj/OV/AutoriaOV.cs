using System.Collections.Generic;
using neo.BRLightREST;
namespace TCDF.Sinj.OV
{
    public class AutoriaOV : metadata
    {
        public AutoriaOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_autoria { get; set; }
        public string nm_autoria { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Autoria
    {
        public string ch_autoria { get; set; }
        public string nm_autoria { get; set; }
    }
}
