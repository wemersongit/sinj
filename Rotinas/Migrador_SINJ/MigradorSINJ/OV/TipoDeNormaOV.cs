using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class TipoDeNormaLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool TCDF { get; set; }
        public bool SEPLAG { get; set; }
        public bool CLDF { get; set; }
        public bool PGDF { get; set; }


        public OrgaoCadastradorLBW OrgaoCadastrador { get; set; }

        /// <summary>
        /// É verdadeiro para os tipos de norma que aparece o campo ”Autoria” pra ser preenchido e pesquisado
        /// </summary>
        public bool Grupo1 { get; set; }

        /// <summary>
        /// É verdadeiros para os tipos de norma que são especificas para ações impetradas no PGDF que 
        ///  podem interferir na vigência de outras normas. Para estes tipos de 
        ///  normas deve estar disponível campos adicionais específicos e alteradas 
        ///  algumas regras de negocio para os campos comuns.
        /// </summary>
        public bool Grupo2 { get; set; }

        /// <summary>
        /// Trata exclusivamente do tipo DODF, pois esta parte do acervo não é uma 
        ///  norma e tem por objetivo apenas disponibilizar o DO do DF para pesquisa.
        /// O DODF não é ato, nem norma e nem ação.
        /// </summary>
        public bool Grupo3 { get; set; }

        /// <summary>
        /// É verdadeiros para os tipos de norma que aparece o campo “Autoria” e “Interessado” pra ser 
        ///  preenchido e pesquisado (exceto as ações da PGDF).
        /// </summary>
        public bool Grupo4 { get; set; }

        /// <summary>
        /// Indica os tipos de normas que podem ser cadastras no VIDES sem a 
        ///  necessidade da norma vinculada já estar cadastra no sistema.
        /// </summary>
        public bool Grupo5 { get; set; }

        public bool Conjunta { get; set; }

        /// <summary>
        /// Indica quais as normas são questionáveis por ações. Observe que durante o 
        ///  cadastro de uma ação pelo PGDF, ao ser preenchido no VIDE qual a norma esta 
        ///  sendo questionada, somente estes tipos estarão disponíveis.
        /// </summary>
        public bool Questionaveis { get; set; }

        /// <summary>
        /// Indica se esse tipo de norma deve usar uma enumeração a partir do orgao
        /// Valor utilizado apenas para criação do numero para a chave para não-duplicação.
        /// </summary>
        public bool ControleDeNumeracaoPorOrgao { get; set; }
    }
    public class TipoDeNormaOV
    {
        public TipoDeNormaOV()
        {
            orgaos_cadastradores = new List<OrgaoCadastradorOV>();
            alteracoes = new List<AlteracaoOV>();
        }

        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        public string ds_tipo_norma { get; set; }

        public List<OrgaoCadastradorOV> orgaos_cadastradores { get; set; }

        public bool in_g1 { get; set; }
        public bool in_g2 { get; set; }
        public bool in_g3 { get; set; }
        public bool in_g4 { get; set; }
        public bool in_g5 { get; set; }

        public bool in_conjunta { get; set; }
        public bool in_apelidavel { get; set; }

        /// <summary>
        /// Indica quais as normas são questionáveis por ações. Observe que durante o 
        ///  cadastro de uma ação pelo PGDF, ao ser preenchido no VIDE qual a norma esta 
        ///  sendo questionada, somente estes tipos estarão disponíveis.
        /// </summary>
        public bool in_questionavel { get; set; }

        /// <summary>
        /// Indica se esse tipo de norma deve usar uma enumeração a partir do orgao
        /// Valor utilizado apenas para criação do numero para a chave para não-duplicação.
        /// </summary>
        public bool in_numeracao_por_orgao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}
