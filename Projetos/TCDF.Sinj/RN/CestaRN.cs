using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TCDF.Sinj.ESUtil;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class CestaRN
    {
        private CestaAD _cestaAd;
        public CestaRN()
        {
            _cestaAd = new CestaAD();
        }
        
        public Result<T> ConsultarEs<T>(HttpContext context)
        {
            return _cestaAd.ConsultarEs<T>(context);
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            return _cestaAd.PesquisarTotalEs(context);
        }
    }
}
