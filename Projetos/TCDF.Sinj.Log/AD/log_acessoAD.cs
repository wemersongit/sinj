using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.AD
{
    public class log_acessoAD
    {
        private AcessoAD<log_acessoOV> _acessoAd;

        public log_acessoAD()
        {
            _acessoAd = new AcessoAD<log_acessoOV>(Config.ValorChave("NmBaseLogAcesso", true));
        }

        public UInt64 Incluir(log_acessoOV olog_acessoOV)
        {
            try
            {
                return _acessoAd.Incluir(olog_acessoOV);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public log_acessoOV ConsultarReg(ulong id_doc)
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