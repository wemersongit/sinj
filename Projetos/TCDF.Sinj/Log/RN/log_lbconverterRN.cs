using neo.BRLightREST;
using TCDF.Sinj.Log.AD;
using TCDF.Sinj.Log.OV;
using util.BRLight;

namespace TCDF.Sinj.Log.RN
{
    public class log_lbconverterRN
    {
        public log_lbconverterOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_lbconverterAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa)
        {
            return new log_lbconverterAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_lbconverterAD().jsonReg(id_doc);
        }

        public bool Excluir(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new log_lbconverterAD().Excluir(id_doc);

        }
    }
}
