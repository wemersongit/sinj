using System;

namespace Exportador_LB_to_ES.AD.Models
{
    public class TipoDeNorma : IComparable<TipoDeNorma>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool TCDF { get; set; }
        public bool SEPLAG { get; set; }
        public bool CLDF { get; set; }
        public bool PGDF { get; set; }


        public OrgaoCadastrador OrgaoCadastrador { get; set; }

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

        public bool EhLei
        {
            get
            {
                //TODO Rever isso
                if (!string.IsNullOrEmpty(Nome))
                {
                    return
                        Nome.Equals("Decreto Legislativo", StringComparison.CurrentCultureIgnoreCase) ||
                        Nome.Equals("Emenda a lei Orgânica", StringComparison.CurrentCultureIgnoreCase) ||
                        Nome.Equals("Lei", StringComparison.CurrentCultureIgnoreCase) ||
                        Nome.Equals("Lei Complementar", StringComparison.CurrentCultureIgnoreCase);
                }
                return false;
            }
        }

        //public bool EhConjunta
        //{
        //    get
        //    {
        //        //TODO Rever isso
        //        return
        //            string.Equals(Nome, "Ato Conjunto", StringComparison.InvariantCultureIgnoreCase) ||
        //            string.Equals(Nome, "Ordem de Servico Conjunta", StringComparison.InvariantCultureIgnoreCase) ||
        //            string.Equals(Nome, "Portaria Conjunta", StringComparison.InvariantCultureIgnoreCase);
        //    }
        //}

        public bool EhDecreto
        {
            get
            {
                //TODO Rever isso
                return string.Equals(Nome, "Decreto", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public bool EhResolucao
        {
            get
            {
                //TODO Rever isso
                return string.Equals(Nome, "Resolução", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public bool EhPortaria
        {
            get
            {
                //TODO Rever isso
                return string.Equals(Nome, "Portaria", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static TipoDeNorma TipoNulo
        {
            get { return new TipoDeNorma(); }
        }

        public int CompareTo(TipoDeNorma other)
        {
            return Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
