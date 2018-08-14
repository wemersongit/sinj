using System.ComponentModel;

namespace TCDF.Sinj.OV
{
    public enum TipoDeDecisaoEnum
    {
        [Description("Liminar Deferida")]
        LiminarDeferida,
        [Description("Liminar Indeferida")]
        LiminarIndeferida,
        [Description("Mérito Procedente")]
        MeritoProcedente,
        [Description("Mérito Improcedente")]
        MeritoImprocedente,
        [Description("Extinta")]
        Extinta,
        [Description("Ajuizado")]
        Ajuizado,
        [Description("Mérito Parcialmente Procedente")]
        MeritoParcialmenteProcedente
    }
}