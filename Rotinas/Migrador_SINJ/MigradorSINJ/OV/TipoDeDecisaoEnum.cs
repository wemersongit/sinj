using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MigradorSINJ.OV
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
        Ajuizado
    }
}
