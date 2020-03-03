using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MigradorSINJ.OV
{
    public enum TipoDeEdicaoEnum
    {
        [Description("Normal")]
        Normal,
        [Description("Extra")]
        Extra,
        [Description("Suplemento")]
        Suplemento
    }
}
