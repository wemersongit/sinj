using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.AD;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.RN
{
    public class log_erroRN
    {
        public UInt64 Incluir(log_erroOV olog_erroOV)
        {
            return new log_erroAD().Incluir(olog_erroOV);
        }

        public log_erroOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_erroAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            return new log_erroAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_erroAD().jsonReg(id_doc);
        }
    }
}
