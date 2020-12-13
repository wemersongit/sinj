using System;

namespace TCDF_REPORT.OV
{
    public class TipoDeNormaOV
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public OrgaoCadastrador OrgaoCadastrador { get; set; }

        /// <summary>
        /// � verdadeiro para os tipos de norma que aparece o campo �Autoria� pra ser preenchido e pesquisado
        /// </summary>
        public bool Grupo1 { get; set; }

        /// <summary>
        /// � verdadeiros para os tipos de norma que s�o especificas para a��es impetradas no PGDF que 
        ///  podem interferir na vig�ncia de outras normas. Para estes tipos de 
        ///  normas deve estar dispon�vel campos adicionais espec�ficos e alteradas 
        ///  algumas regras de negocio para os campos comuns.
        /// </summary>
        public bool Grupo2 { get; set; }

        /// <summary>
        /// Trata exclusivamente do tipo DODF, pois esta parte do acervo n�o � uma 
        ///  norma e tem por objetivo apenas disponibilizar o DO do DF para pesquisa.
        /// O DODF n�o � ato, nem norma e nem a��o.
        /// </summary>
        public bool Grupo3 { get; set; }

        /// <summary>
        /// � verdadeiros para os tipos de norma que aparece o campo �Autoria� e �Interessado� pra ser 
        ///  preenchido e pesquisado (exceto as a��es da PGDF).
        /// </summary>
        public bool Grupo4 { get; set; }

        /// <summary>
        /// Indica os tipos de normas que podem ser cadastras no VIDES sem a 
        ///  necessidade da norma vinculada j� estar cadastra no sistema.
        /// </summary>
        public bool Grupo5 { get; set; }

        public bool Conjunta { get; set; }

        /// <summary>
        /// Indica quais as normas s�o question�veis por a��es. Observe que durante o 
        ///  cadastro de uma a��o pelo PGDF, ao ser preenchido no VIDE qual a norma esta 
        ///  sendo questionada, somente estes tipos estar�o dispon�veis.
        /// </summary>
        public bool Questionaveis { get; set; }

        /// <summary>
        /// Indica se esse tipo de norma deve usar uma enumera��o a partir do orgao
        /// Valor utilizado apenas para cria��o do numero para a chave para n�o-duplica��o.
        /// </summary>
        public bool ControleDeNumeracaoPorOrgao { get; set; }

        public bool EhLei
        {
            get
            {
                //TODO Rever isso
                return
                    string.Equals(Nome, "Decreto Legislativo", StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(Nome, "Emenda a lei Org�nica", StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(Nome, "Lei", StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(Nome, "Lei Complementar", StringComparison.CurrentCultureIgnoreCase);
            }
        }

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
                return string.Equals(Nome, "Resolu��o", StringComparison.InvariantCultureIgnoreCase);
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

        public override string ToString()
        {
            return Nome;
        }
    }
}