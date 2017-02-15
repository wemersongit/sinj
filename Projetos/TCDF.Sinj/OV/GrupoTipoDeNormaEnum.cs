using System;
using System.ComponentModel;

namespace TCDF.Sinj.OV
{
    [Serializable]
    [Flags]
    public enum GrupoTipoDeNormaEnum
    {
        /// <summary>
        /// É verdadeiro para os tipos de norma que aparece o campo ”Autoria” pra ser preenchido e pesquisado
        /// </summary>
        [Description("Grupo1")]
        G1 = 0,
        /// <summary>
        /// É verdadeiros para os tipos de norma que são especificas para ações impetradas no PGDF que 
        ///  podem interferir na vigência de outras normas. Para estes tipos de 
        ///  normas deve estar disponível campos adicionais específicos e alteradas 
        ///  algumas regras de negocio para os campos comuns.
        /// </summary>
        [Description("Grupo2")]
        G2 = 1,
        /// <summary>
        /// Trata exclusivamente do tipo DODF, pois esta parte do acervo não é uma 
        ///  norma e tem por objetivo apenas disponibilizar o DO do DF para pesquisa.
        /// O DODF não é ato, nem norma e nem ação.
        /// </summary>
        [Description("Grupo3")]
        G3 = 2,
        /// <summary>
        /// É verdadeiros para os tipos de norma que aparece o campo “Autoria” e “Interessado” pra ser 
        ///  preenchido e pesquisado (exceto as ações da PGDF).
        /// </summary>
        [Description("Grupo4")]
        G4 = 4,
        /// <summary>
        /// Indica os tipos de normas que podem ser cadastras no VIDES sem a 
        ///  necessidade da norma vinculada já estar cadastra no sistema.
        /// </summary>
        [Description("Grupo5")]
        G5 = 8,
    }
}