using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class TipoDeNormaRN
    {
        private TipoDeNormaAD _tipoDeNormaAd;

        public TipoDeNormaRN()
        {
            _tipoDeNormaAd = new TipoDeNormaAD();
        }

        public List<TipoDeNormaLBW> BuscarTiposDeNormaLBW()
        {
            return _tipoDeNormaAd.BuscarTiposDeNormaLBW();
        }

        public ulong Incluir(TipoDeNormaOV tipoDeNormaOv)
        {
            return _tipoDeNormaAd.Incluir(tipoDeNormaOv);
        }
    }
}
