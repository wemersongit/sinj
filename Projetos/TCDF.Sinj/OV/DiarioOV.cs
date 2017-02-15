using System;
using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class DiarioOV : metadata
    {
		public DiarioOV(){
			alteracoes = new List<AlteracaoOV>();
            arquivos = new List<ArquivoDiario>();
            ar_diario = new ArquivoOV();
		}

        public string ch_diario { get; set; }
        public string ch_para_nao_duplicacao { get; set; }
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string ch_tipo_edicao { get; set; }
        public string nm_tipo_edicao { get; set; }
        public string nm_diferencial_edicao { get; set; }
        public int nr_diario { get; set; }
        /// <summary>
        /// Letra para conseguir cadastrar diario com o mesmo número.
        /// </summary>
        public string cr_diario { get; set; }
        public string secao_diario { get; set; }
        public string dt_assinatura { get; set; }
        public int nr_ano { get; set; }
        public bool st_pendente { get; set; }
        public string ds_pendencia { get; set; }

        public bool st_suplemento { get; set; }
        public string nm_diferencial_suplemento { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        /// <summary>
        /// Campo legado, manter para recuperar os antigos mas os novos devem ser salvos em arquivos.arquivo_diario
        /// </summary>
        public ArquivoOV ar_diario { get; set; }
        public List<ArquivoDiario> arquivos { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class ArquivoDiario {
        public ArquivoDiario()
        {
            arquivo_diario = new ArquivoOV();
            ds_arquivo = "";
        }
        public ArquivoOV arquivo_diario { get; set; }
        public string ds_arquivo { get; set; }
    }
}