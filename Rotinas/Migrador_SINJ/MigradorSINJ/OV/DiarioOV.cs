using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class DiarioLBW
    {
        public string Id { get; set; }
        public string Numero { get; set; }
        public string Sessao { get; set; }
        public string OrgaoCadastrador { get; set; }
        public string DataDaAssinatura { get; set; }

        public string UsuarioQueCadastrou { get; set; }
        public string DataDoCadastro { get; set; }

        public string UsuarioDaUltimaAlteracao { get; set; }
        public string DataDaUltimaAlteracao { get; set; }
        public string ChaveParaNaoDuplicacao { get; set; }
        public string SituacaoQuantoAPendencia { get; set; }
        public string CaminhoArquivoTexto { get; set; }

    }

    public class DiarioOV
    {
        public DiarioOV()
        {
            alteracoes = new List<AlteracaoOV>();
            ar_diario = new ArquivoOV();
            _metadata = new metadata();
        }
        public metadata _metadata { get; set; }
        public string ch_diario { get; set; }
        public string ch_para_nao_duplicacao { get; set; }
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public int nr_diario { get; set; }
        /// <summary>
        /// Letra para conseguir cadastrar diario com o mesmo número.
        /// </summary>
        public string cr_diario { get; set; }
        public string secao_diario { get; set; }
        public string dt_assinatura { get; set; }
        public bool st_pendente { get; set; }
        public string ds_pendencia { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }

        public ArquivoOV ar_diario { get; set; }

        public List<AlteracaoOV> alteracoes { get; set; }
    }
}
