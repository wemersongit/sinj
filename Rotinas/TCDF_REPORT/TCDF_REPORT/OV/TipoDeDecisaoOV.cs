using System.ComponentModel;

namespace TCDF_REPORT.OV
{
    public enum TipoDeDecisaoOV
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
        Extinta
    }
}