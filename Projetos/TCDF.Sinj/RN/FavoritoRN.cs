using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TCDF.Sinj.ESUtil;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class FavoritoRN
    {
        private DocEs _docEs;
        private FavoritoAD _favoritoAd;
        public FavoritoRN()
        {
            _docEs = new DocEs();
            _favoritoAd = new FavoritoAD();
        }

        public Result<T> ConsultarEs<T>(HttpContext context)
        {
            return _favoritoAd.ConsultarEs<T>(context);
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            return _favoritoAd.PesquisarTotalEs(context);
        }
    }
}
