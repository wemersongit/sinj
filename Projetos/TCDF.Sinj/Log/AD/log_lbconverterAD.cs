using neo.BRLightREST;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.AD
{
    public class log_lbconverterAD
    {
        private AcessoAD<log_lbconverterOV> _acessoAd;

        public log_lbconverterAD()
        {
            _acessoAd = new AcessoAD<log_lbconverterOV>(Config.ValorChave("NmBaseLogLBConverter", true));
        }

        public log_lbconverterOV ConsultarReg(ulong id_doc)
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

        public bool Excluir(ulong id)
        {
            return _acessoAd.Excluir(id);
        }
    }
}
