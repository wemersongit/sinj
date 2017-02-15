using System;
using neo.BRLightREST;
using TCDF.Sinj.Log.AD;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.RN
{
    public class log_operacaoRN
    {
        public UInt64 Incluir(log_operacaoOV olog_operacaoOV)
        {

            Params.CheckNotNullOrEmpty("dt_inicio", olog_operacaoOV.dt_inicio);
            return new log_operacaoAD().Incluir(olog_operacaoOV);
        }

        public bool AlterarPath_dt_fim(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_operacaoAD().AlterarPath_dt_fim(id_doc, "dt_fim", DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"));
        }

        public log_operacaoOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_operacaoAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            return new log_operacaoAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_operacaoAD().jsonReg(id_doc);
        }
    }
}