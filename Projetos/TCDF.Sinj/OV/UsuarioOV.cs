using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class UsuarioOV : metadata
    {

        public UsuarioOV()
        {
            grupos = new List<string>();
            alteracoes = new List<AlteracaoOV>();
        }

        public string nm_login_usuario { get; set; }
        public string nm_usuario { get; set; }
        public string senha_usuario { get; set; }
        public string email_usuario { get; set; }
        public string pagina_inicial { get; set; }
        public string ds_pagina_inicial { get; set; }
        public string ch_tema { get; set; }
        public string dt_ultimo_login { get; set; }
        public string ch_push_usuario { get; set; }

        public bool st_usuario { get; set; }
        public bool in_alterar_senha { get; set; }

        public string ch_perfil { get; set; }
        public string nm_perfil { get; set; }
        public List<string> grupos { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        public int nr_tentativa_login { get; set; }

        public OrgaoCadastrador orgao_cadastrador { get; set; }

    }
}
