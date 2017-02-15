using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.RN
{
    public class ExcluidoRN
    {
        private ExcluidoAD _excluidoAd;

        public ExcluidoRN()
        {
            _excluidoAd = new ExcluidoAD();
        }

        public ulong Incluir(ExcluidoOV excluidoOv)
        {
            return _excluidoAd.Incluir(excluidoOv);
        }

        public string jsonReg(Pesquisa query)
        {
            return _excluidoAd.JsonReg(query);
        }

        public bool Excluir(ulong id_doc)
        {
            return _excluidoAd.Excluir(id_doc);
        }

        public ExcluidoOV ConsultarReg(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new ExcluidoAD().ConsultarReg(id_doc);
        }
    }
}
