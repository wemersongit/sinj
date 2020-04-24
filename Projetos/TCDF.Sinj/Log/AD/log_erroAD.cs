using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.AD
{
    public class log_erroAD
    {
        private AcessoAD<log_erroOV> _AcessoAd;

        public log_erroAD()
        {
            _AcessoAd = new AcessoAD<log_erroOV>(Config.ValorChave("NmBaseLogErro", true));
        }

        public UInt64 Incluir(log_erroOV olog_erroOV)
        {
            try
            {
                return _AcessoAd.Incluir(olog_erroOV);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public log_erroOV ConsultarReg(ulong id_doc)
        {
            return _AcessoAd.ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa opesquisa)
        {
            return _AcessoAd.jsonReg(opesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            return _AcessoAd.jsonReg(id_doc);
        }
    }
}
