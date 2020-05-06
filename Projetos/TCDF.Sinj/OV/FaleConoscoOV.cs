using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class FaleConoscoOV : metadata
    {
        public FaleConoscoOV()
        {
            mensagens = new List<FaleConoscoMensagemResposta>();
        }
        public string ch_chamado { get; set; }
        public string ch_controle_excesso_email { get; set; }
        public string nm_user { get; set; }
        public string ds_email { get; set; }
        public string ds_assunto { get; set; }
        public string ds_msg { get; set; }
        public string nr_telefone { get; set; }
        public string dt_inclusao { get; set; }
        //Novo, Recebido, Finalizado
        public string st_atendimento { get; set; }
        public string nm_orgao_cadastrador_atribuido { get; set; }
        public string dt_recebido { get; set; }
        public string dt_finalizado { get; set; }
        public string nm_login_usuario_atendimento { get; set; }
        public string nm_usuario_atendimento { get; set; }
        public string print { get; set; }
        public string ds_url_pagina { get; set; }

        public List<FaleConoscoMensagemResposta> mensagens { get; set; }
    }

    public class FaleConoscoMensagemResposta {
        public string ds_assunto_resposta { get; set; }
        public string ds_msg_resposta { get; set; }
        public string nm_login_usuario_resposta { get; set; }
        public string nm_usuario_resposta { get; set; }
        public string dt_resposta { get; set; }
    }
}
