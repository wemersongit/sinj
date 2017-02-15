using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.AD
{
    public class log_operacaoAD
    {
        private AcessoAD<log_operacaoOV> _acessoAd;

        public log_operacaoAD()
        {
            _acessoAd = new AcessoAD<log_operacaoOV>(Config.ValorChave("NmBaseLogOperacao", true));
        }

        public UInt64 Incluir(log_operacaoOV olog_operacaoOV)
        {
            try
            {
                return _acessoAd.Incluir(olog_operacaoOV);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public log_operacaoOV ConsultarReg(ulong id_doc)
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

        public bool AlterarPath_dt_fim(ulong id_doc, string path, string valor)
        {
            var resultado = _acessoAd.pathPut(id_doc, path, valor, null);
            return (resultado.ToUpper() == "UPDATED");
        }
    }
}