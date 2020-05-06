using System;
using neo.BRLightREST;
using System.Collections.Generic;

namespace TCDF.Sinj.OV
{
    public class Decisao
    {
        public TipoDeDecisaoEnum in_decisao { get; set; }
        public string nm_decisao { get { return util.BRLight.Util.GetEnumDescription(this.in_decisao); } }
        public string dt_decisao { get; set; }
        public string ds_complemento { get; set; }
    }

}
