using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;

namespace MigradorSINJ.RN
{
    public class SituacaoRN
    {
        private SituacaoAD _situacaoAd;

        public SituacaoRN()
        {
            _situacaoAd = new SituacaoAD();
        }

        public List<SituacaoLBW> BuscarSituacoesLBW()
        {
            return _situacaoAd.BuscarSituacoesLBW();
        }

        public ulong Incluir(SituacaoOV situacaoOv)
        {
            return _situacaoAd.Incluir(situacaoOv);
        }
    }
}
