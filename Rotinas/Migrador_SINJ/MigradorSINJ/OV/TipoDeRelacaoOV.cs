using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class TipoDeRelacaoLBW
    {
        public int Oid { get; set; }
        public string Conteudo { get; set; }
        public string Descricao { get; set; }
        public string TextoParaAlterador { get; set; }
        public string TextoParaAlterado { get; set; }
        public int Importancia { get; set; }

        /// <summary>
        /// Esse campo funciona como um flag, é true apenas quando o tipo de relação é usada para Ações com outras normas;
        /// </summary>
        public bool RelacaoDeAcao { get; set; }

        /// <summary>
        /// Esse campo apenas é preenchido durante a migração do SILEG.
        /// Serve para guardar informações sobre pendencia durante a conversão entre Tipo De Relação do SILEG para o Tipo do SINJ.
        /// </summary>
        public string Pendencia { get; set; }
    }
    public class TipoDeRelacaoOV
    {
        public TipoDeRelacaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_tipo_relacao { get; set; }
        public string nm_tipo_relacao { get; set; }
        public string ds_tipo_relacao { get; set; }
        public string ds_texto_para_alterador { get; set; }
        public string ds_texto_para_alterado { get; set; }
        public int nr_importancia { get; set; }

        /// <summary>
        /// Esse campo funciona como um flag, é true apenas quando o tipo de relação é usada para Ações com outras normas;
        /// </summary>
        public bool in_relacao_de_acao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}
