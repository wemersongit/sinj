using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class TipoDeRelacaoRN
    {
        private TipoDeRelacaoAD _tipoDeRelacaoAd;

        public TipoDeRelacaoRN()
        {
            _tipoDeRelacaoAd = new TipoDeRelacaoAD();
        }

        public List<TipoDeRelacaoLBW> BuscarTiposDeRelacaoLBW()
        {
            return _tipoDeRelacaoAd.BuscarTiposDeRelacaoLBW();
        }

        public ulong Incluir(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            return _tipoDeRelacaoAd.Incluir(tipoDeRelacaoOv);
        }
    }
}
