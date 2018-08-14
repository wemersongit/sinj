using System.ComponentModel;

namespace TCDF.Sinj.OV
{
    public enum TipoDeDecisaoEnum
    {
        [Description("Liminar Deferida")]
        LiminarDeferida,
        [Description("Liminar Indeferida")]
        LiminarIndeferida,
        [Description("M�rito Procedente")]
        MeritoProcedente,
        [Description("M�rito Improcedente")]
        MeritoImprocedente,
        [Description("Extinta")]
        Extinta,
        [Description("Ajuizado")]
        Ajuizado,
        [Description("M�rito Parcialmente Procedente")]
        MeritoParcialmenteProcedente
    }
}