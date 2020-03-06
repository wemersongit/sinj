using System.ComponentModel;

namespace TCDF_REPORT.OV
{
    public enum TipoDeDecisaoOV
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
        Extinta
    }
}