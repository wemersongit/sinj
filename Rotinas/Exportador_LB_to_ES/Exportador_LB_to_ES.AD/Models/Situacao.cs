using System;
using System.ComponentModel;
using System.Reflection;

namespace Exportador_LB_to_ES.AD.Models
{
    public enum Situacao
    {
        [Description("Revogado")]
        Revogado = 2,

        [Description("Sustado")]
        Sustado = 3,

        [Description("Anulado")]
        Anulado = 4,

        [Description("Tornado Sem Efeito")]
        TornadoSemEfeito = 5,

        [Description("Inconstitucional")]
        Inconstitucional = 6,

        [Description("Alterado")]
        Alterado = 8,

        [Description("Sem Revogação Expressa")]
        SemRevogacaoExpressa = 10,

        [Description("Suspenso")]
        Suspenso = 12,

        [Description("Pendente")]
        Pendente,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Aguardando Julgamento")]
        AguardandoJulgamento,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Aguardando Julgamento (Lnar. Indeferida)")]
        AguardandoJulgamento1,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Aguardando Julgamento (Lnar. Deferida)")]
        AguardandoJulgamento2,

        /// <remarks> Esse valor não existia. Foi adicionado por Eduardo em Brasília-DF. </remarks>
        [Description("Liminar Deferida")]
        LiminarDeferida,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Julgada Procedente")]
        JulgadoProcedente,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Julgada Improcedente")]
        JulgadoImprocedente,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Extinta")]
        Extinta,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Ajuizado")]
        Ajuizado,

        /// <remarks> Caso na base mude o nome da relação. Esse tipo fica sem sentido. </remarks>
        [Description("Suspenso Parcialmente")]
        SuspensoParcialmente,

        //Note: Esse valor para situação não deveria existir conforme a especificação do sistema.
        //[Description("Em Vigor")]
        //EmVigor = 99

    };

    public static class SituacaoUtil
    {
        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetEnumDescription(string value)
        {
            return GetEnumDescription((Situacao)Enum.Parse(typeof(Situacao), value));
        }

    }
}
