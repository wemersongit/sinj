using System.ComponentModel;

namespace Exportador_LB_to_ES.AD.Models
{
    public enum TipoDeDecisao
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
