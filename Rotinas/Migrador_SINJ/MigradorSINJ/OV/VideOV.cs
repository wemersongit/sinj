using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{

    public class VideLBW
    {
        #region Atributos do Vide

        public Guid Id { get; set; }
        public int IdDaNormaPosterior { get; set; }
        public string NumeroDaNormaPosterior { get; set; }
        public string ComentarioVide { get; set; }
        public string PaginaDaPublicacaoPosterior { get; set; }
        public string ColunaDaPublicacaoPosterior { get; set; }
        public string DataDaNormaPosterior { get; set; }
        public string DataDePublicacaoPosterior { get; set; }

        #endregion

        #region Informações usadas apenas se a norma alteradora não for cadastrada no sistema

        /// <summary>
        /// Identifica a denominação da norma com relação de vinculo.
        /// </summary>
        public int TipoDeNorma { get; set; }

        /// <summary>
        /// Identifica a fonte publicadora da norma que tem a relação de vinculo.
        /// </summary>
        public string TipoDeFonte { get; set; }

        #endregion

        /// <summary>
        /// Contem qual o tipo de relação de vinculo existe entre a norma anterior e a outra posterior.
        /// </summary>
        public int TipoDeVinculo { get; set; }

        #region Dados da Norma Posterior

        /// <summary>
        /// Contem o numero do artigo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ArtigoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do parágrafo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ParagrafoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do Inciso que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string IncisoDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero da Alínea que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AlineaDaNormaPosterior { get; set; }

        /// <summary>
        /// Contem o numero do Item que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ItemDaNormaPosterior { get; set; }

        /// <summary>
        /// Define que é o CAPUT que tem uma ralação de vinculo com outra norma anterior.
        /// </summary>
        public string CaputDaNormaPosterior { get; set; }

        /// <summary>
        ///	Contem a identificação do Anexo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AnexoDaNormaPosterior { get; set; }

        /// <summary>
        /// Parametro auxiliar para comparar os vides.
        /// </summary>
        public bool VideAlterador { get; set; }

        #endregion

        #region Dados da Norma Anterior

        /// <summary>
        /// Contem o numero do artigo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ArtigoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do parágrafo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ParagrafoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do Inciso que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string IncisoDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero da Alínea que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AlineaDaNormaAnterior { get; set; }

        /// <summary>
        /// Contem o numero do Item que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string ItemDaNormaAnterior { get; set; }

        /// <summary>
        /// Define que é o CAPUT que tem uma ralação de vinculo com outra norma anterior.
        /// </summary>
        public string CaputDaNormaAnterior { get; set; }

        /// <summary>
        ///	Contem a identificação do Anexo que se deseja marcar uma relação de vinculo com outra norma anterior.
        /// </summary>
        public string AnexoDaNormaAnterior { get; set; }

        /// <summary>
        /// Retorna/Define a chave da norma posterior (<see cref="Norma.ChaveParaNaoDuplicacao"/>)
        /// </summary>
        public string ChaveDaNormaPosterior { get; set; }

        #endregion
    }

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
        public string caput_norma_vide { get; set; }
        public string anexo_norma_vide { get; set; }

        public string artigo_norma_vide_outra { get; set; }
        public string paragrafo_norma_vide_outra { get; set; }
        public string inciso_norma_vide_outra { get; set; }
        public string alinea_norma_vide_outra { get; set; }
        public string item_norma_vide_outra { get; set; }
        public string caput_norma_vide_outra { get; set; }
        public string anexo_norma_vide_outra { get; set; }
    }
}
