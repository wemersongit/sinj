using System;
using System.ComponentModel;

namespace TCDF_REPORT.OV
{
    [Serializable]
    [Flags]
    public enum OrgaoCadastrador
    {
        [Description("Nenhum")]
        NENHUM = 0,
        [Description("SEPLAG")]
        SEPLAG = 1,
        [Description("CLDF")]
        CLDF = 2,
        [Description("TCDF")]
        TCDF = 4,
        [Description("PGDF")]
        PGDF = 8,
        [Description("TODOS")]
        TODOS = 16,
    }
}