using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class DecisaoLBW
    {
        public long IdDaNorma { get; set; }
        public long Id { get; set; }
        public string DataDaPublicacao { get; set; }
        public TipoDeDecisaoEnum Tipo { get; set; }
        public string Complemento { get; set; }
    }
    public class Decisao
    {
        public TipoDeDecisaoEnum in_decisao { get; set; }
        public string nm_decisao { get { return util.BRLight.Util.GetEnumDescription(in_decisao); } }
        public string dt_decisao { get; set; }
        public string ds_complemento { get; set; }
    }
}
