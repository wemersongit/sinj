using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class PerfilRN
    {
        private PerfilAD _perfilAd;

        public PerfilRN()
        {
            _perfilAd = new PerfilAD();
        }

        public string JsonReg(Pesquisa query)
        {
            return _perfilAd.JsonReg(query);
        }
    }
}
