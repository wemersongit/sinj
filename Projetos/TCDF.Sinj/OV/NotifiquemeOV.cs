using System;
using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class NotifiquemeOV : metadata
    {
        public NotifiquemeOV(){
            normas_monitoradas = new List<NormaMonitoradaPushOV>();
            criacao_normas_monitoradas = new List<CriacaoDeNormaMonitoradaPushOV>();
            termos_diarios_monitorados = new List<TermoDiarioMonitoradoPushOV>();
            favoritos = new List<string>();
        }

        /// <summary>
        /// Usado para login no push no caso de usuários não cadastrados no sistema, ou seja, não UsuarioOV
        /// </summary>
        public string email_usuario_push { get; set; }

        /// <summary>
        /// Indentifica o nome do usuário (UsuarioOV ou não)
        /// </summary>
        public string nm_usuario_push { get; set; }

        /// <summary>
        /// Usado para login no push no caso de usuários não cadastrados no sistema, ou seja, não UsuarioOV
        /// </summary>
        public string senha_usuario_push { get; set; }

        // Contém uma lista de chaves concatenadas com identificadores de base (norma ou diario). Ex.: norma_78993
        public List<string> favoritos { get; set; }

        public List<TermoDiarioMonitoradoPushOV> termos_diarios_monitorados { get; set; }
        public List<NormaMonitoradaPushOV> normas_monitoradas { get; set; }
        public List<CriacaoDeNormaMonitoradaPushOV> criacao_normas_monitoradas { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo ou não.
        /// </summary>
        public bool st_push { get; set; }
    }

    public class TermoDiarioMonitoradoPushOV
    {
        public string ch_termo_diario_monitorado { get; set; }
        public string ch_tipo_fonte_diario_monitorado { get; set; }
        public string nm_tipo_fonte_diario_monitorado { get; set; }
        public string ds_termo_diario_monitorado { get; set; }
        public string dt_cadastro_termo_diario_monitorado { get; set; }
        public bool st_termo_diario_monitorado { get; set; }
    }

    public class NormaMonitoradaPushOV
    {
        public string ch_norma_monitorada { get; set; }
        public string ch_tipo_norma_monitorada { get; set; }
        public string nm_tipo_norma_monitorada { get; set; }
        public string nr_norma_monitorada { get; set; }
        public string dt_assinatura_norma_monitorada { get; set; }
        public string dt_cadastro_norma_monitorada { get; set; }
        public bool st_norma_monitorada { get; set; }
    }

    public class CriacaoDeNormaMonitoradaPushOV
    {
        public string ch_criacao_norma_monitorada { get; set; }

        public string ch_tipo_norma_criacao { get; set; }
        public string nm_tipo_norma_criacao { get; set; }

        public string primeiro_conector_criacao { get; set; }

        public string ch_orgao_criacao { get; set; }
        public string nm_orgao_criacao { get; set; }

        public string segundo_conector_criacao { get; set; }

        public string ch_termo_criacao { get; set; }
        public string ch_tipo_termo_criacao { get; set; }
        public string nm_termo_criacao { get; set; }

        public bool st_criacao { get; set; }
    }
}