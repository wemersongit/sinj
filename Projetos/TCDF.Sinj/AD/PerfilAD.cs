using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class PerfilAD
    {
        private AcessoAD<PerfilOV> _acessoAd;

        public PerfilAD()
        {
            _acessoAd = new AcessoAD<PerfilOV>(util.BRLight.Util.GetVariavel("NmBasePerfil", true));
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }
    }
}
