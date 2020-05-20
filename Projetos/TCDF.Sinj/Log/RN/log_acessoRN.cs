using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.AD;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.RN
{
    public class log_acessoRN
    {
        public UInt64 Incluir(log_acessoOV olog_acessoOV)
        {
            Params.CheckNotNullOrEmpty("nm_apelido", olog_acessoOV.nm_aplicacao);
            Params.CheckNotNullOrEmpty("nm_aplicacao", olog_acessoOV.nm_aplicacao);
            Params.CheckNotNullOrEmpty("dt_exclusao", olog_acessoOV.dt_acesso);

            return new log_acessoAD().Incluir(olog_acessoOV);
        }

        public log_acessoOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_acessoAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            return new log_acessoAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_acessoAD().jsonReg(id_doc);
        }
    }
}
