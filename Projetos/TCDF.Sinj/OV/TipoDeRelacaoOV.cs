using neo.BRLightREST;
using System.Collections.Generic;
namespace TCDF.Sinj.OV
{
    public class TipoDeRelacaoOV : metadata
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

    public class TipoDeRelacao
    {
        public string ch_tipo_relacao { get; set; }
        public string nm_tipo_relacao { get; set; }
        public string ds_tipo_relacao { get; set; }
        public string ds_texto_para_alterador { get; set; }
        public string ds_texto_para_alterado { get; set; }
        public int nr_importancia { get; set; }
        public bool in_relacao_de_acao { get; set; }
    }
}