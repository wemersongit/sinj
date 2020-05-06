using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class RequerenteRN
    {
        private RequerenteAD _requerenteAd;

        public RequerenteRN()
        {
            _requerenteAd = new RequerenteAD();
        }

        public List<RequerenteLBW> BuscarRequerentesLBW()
        {
            return _requerenteAd.BuscarRequerentesLBW();
        }

        public ulong Incluir(RequerenteOV requerenteOv)
        {
            return _requerenteAd.Incluir(requerenteOv);
        }
    }
}
