using neo.BRLightREST;
using TCDF.Sinj.Log.AD;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.RN
{
    public class log_lbindexRN
    {
        public log_lbindexOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_lbindexAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            return new log_lbindexAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_lbindexAD().jsonReg(id_doc);
        }
    }
}
