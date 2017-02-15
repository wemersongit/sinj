using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class TipoDePublicacaoRN
    {
        private TipoDePublicacaoAD _tipoDePublicacaoAd;

        public TipoDePublicacaoRN()
        {
            _tipoDePublicacaoAd = new TipoDePublicacaoAD();
        }

        public List<TipoDePublicacaoLBW> BuscarTiposDePublicacaoLBW()
        {
            return _tipoDePublicacaoAd.BuscarTiposDePublicacaoLBW();
        }

        public ulong Incluir(TipoDePublicacaoOV tipoDePublicacaoOv)
        {
            return _tipoDePublicacaoAd.Incluir(tipoDePublicacaoOv);
        }
    }
}
