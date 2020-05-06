using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class PerfilOV : metadata
    {
        public PerfilOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_perfil { get; set; }
        public string nm_perfil { get; set; }
        public string ds_perfil { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }


    }
}
