using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class RequeridoRN
    {
        private RequeridoAD _requeridoAd;

        public RequeridoRN()
        {
            _requeridoAd = new RequeridoAD();
        }

        public List<RequeridoLBW> BuscarRequeridosLBW()
        {
            return _requeridoAd.BuscarRequeridosLBW();
        }

        public ulong Incluir(RequeridoOV requeridoOv)
        {
            return _requeridoAd.Incluir(requeridoOv);
        }
    }
}
