using neo.BRLightREST;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.AD
{
    public class log_lbindexAD
    {
        private AcessoAD<log_lbindexOV> _acessoAd;

        public log_lbindexAD()
        {
            _acessoAd = new AcessoAD<log_lbindexOV>(Config.ValorChave("NmBaseLogLBIndex", true));
        }

        public log_lbindexOV ConsultarReg(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa opesquisa)
        {
            return _acessoAd.jsonReg(opesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }
    }
}
