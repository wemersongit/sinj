using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class InteressadoRN
    {
        private InteressadoAD _interessadoAd;

        public InteressadoRN()
        {
            _interessadoAd = new InteressadoAD();
        }

        public List<InteressadoLBW> BuscarInteressadosLBW()
        {
            return _interessadoAd.BuscarInteressadosLBW();
        }

        public ulong Incluir(InteressadoOV interessadoOv)
        {
            return _interessadoAd.Incluir(interessadoOv);
        }
    }
}
