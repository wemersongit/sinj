using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class AutoriaRN
    {
        private AutoriaAD _autoriaAd;

        public AutoriaRN()
        {
            _autoriaAd = new AutoriaAD();
        }

        public List<AutoriaLBW> BuscarAutoriasLBW()
        {
            return _autoriaAd.BuscarAutoriasLBW();
        }

        public ulong Incluir(AutoriaOV autoriaOv)
        {
            return _autoriaAd.Incluir(autoriaOv);
        }
    }
}
