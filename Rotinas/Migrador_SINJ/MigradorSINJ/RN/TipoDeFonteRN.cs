using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class TipoDeFonteRN
    {
        private TipoDeFonteAD _tipoDeFonteAd;

        public TipoDeFonteRN()
        {
            _tipoDeFonteAd = new TipoDeFonteAD();
        }

        public List<TipoDeFonteLBW> BuscarTiposDeFonteLBW()
        {
            return _tipoDeFonteAd.BuscarTiposDeFonteLBW();
        }

        public ulong Incluir(TipoDeFonteOV tipoDeFonteOv)
        {
            return _tipoDeFonteAd.Incluir(tipoDeFonteOv);
        }
    }
}
