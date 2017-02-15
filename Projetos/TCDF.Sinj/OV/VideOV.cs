using System;
using System.Data;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    /// <summary>
    /// Replicar o vide em ambas as normas, não só na norma afetada. Isso vai render um ganho na busca e na recuperação.
    /// Tratar os dados que cada uma vai exibir durante a persistencia no banco, ou seja, a norma afetada fica com o campos preenchidos
    /// para na hora de exibir os vides já estarem tratados os campos em ambas as normas.
    /// </summary>
    public class Vide
    {
        public string ch_vide { get; set; }
        public string ds_comentario_vide { get; set; }
        /// <summary>
        /// Se verdadeiro então a norma corrente é afetada pelo vide.
        /// </summary>
        public bool in_norma_afetada { get; set; }

        //Tipo de Relação
        public string ch_tipo_relacao { get; set; }
        public string nm_tipo_relacao { get; set; }
        /// <summary>
        /// Se a norma corrente for a afetada use o texto ds_texto_para_alterado, se não, use ds_texto_para_alterador, oriundos de de TipoDeRelacao
        /// </summary>
        public string ds_texto_relacao { get; set; }
        public bool in_relacao_de_acao { get; set; }

        //Dados da outra Norma
        public string dt_publicacao_fonte_norma_vide { get; set; }
        public string ch_norma_vide { get; set; }
        public string ch_tipo_norma_vide { get; set; }
        public string nm_tipo_norma_vide { get; set; }
        public string nr_norma_vide { get; set; }
        public string dt_assinatura_norma_vide { get; set; }
        public string pagina_publicacao_norma_vide { get; set; }
        public string coluna_publicacao_norma_vide { get; set; }
        public string ch_tipo_fonte_norma_vide { get; set; }
        public string nm_tipo_fonte_norma_vide { get; set; }

        public string artigo_norma_vide { get; set; }
        public string paragrafo_norma_vide { get; set; }
        public string inciso_norma_vide { get; set; }
        public string alinea_norma_vide { get; set; }
        public string item_norma_vide { get; set; }
        public Caput caput_norma_vide { get; set; }
        public string anexo_norma_vide { get; set; }

        public string artigo_norma_vide_outra { get; set; }
        public string paragrafo_norma_vide_outra { get; set; }
        public string inciso_norma_vide_outra { get; set; }
        public string alinea_norma_vide_outra { get; set; }
        public string item_norma_vide_outra { get; set; }
        public Caput caput_norma_vide_outra { get; set; }
        public string anexo_norma_vide_outra { get; set; }
    }

    public class Caput
    {
        public string[] caput { get; set; }
        public string id_file { get; set; }
        public string ch_norma { get; set; }
        public string ds_norma { get; set; }
        public string path { get; set; }
        public string linkname { get; set; }
        public string[] texto_antigo { get; set; }
        public string[] texto_novo { get; set; }
        public string link { get; set; }
        public string filename { get; set; }
        public string dt_inicio_vigor { get; set; }
        public string st_aguardar_vigor { get; set; }
        public bool bAcrescimo { get; set; }
        public bool bRenumeracao { get; set; }
    }

}