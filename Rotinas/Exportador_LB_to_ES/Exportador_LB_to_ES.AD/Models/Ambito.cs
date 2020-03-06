using System.ComponentModel;

namespace Exportador_LB_to_ES.AD.Models
{
    public enum Ambito
    {
        [Description("Federal")]
        Federal,
        [Description("Estadual")]
        Estadual,
        [Description("Municipal")]
        Municipal,
        [Description("Distrito Federal")]
        DistritoFederal
    };
}
