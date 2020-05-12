using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MigradorSINJ.OV
{
    [Serializable]
    [Flags]
    public enum OrgaoCadastradorLBW
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
    public class OrgaoCadastradorOV
    {
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
    }
}
