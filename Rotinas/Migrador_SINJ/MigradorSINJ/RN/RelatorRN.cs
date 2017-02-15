using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class RelatorRN
    {
        private RelatorAD _relatorAd;

        public RelatorRN()
        {
            _relatorAd = new RelatorAD();
        }

        public List<RelatorLBW> BuscarRelatorsLBW()
        {
            return _relatorAd.BuscarRelatorsLBW();
        }

        public ulong Incluir(RelatorOV relatorOv)
        {
            return _relatorAd.Incluir(relatorOv);
        }
    }
}
