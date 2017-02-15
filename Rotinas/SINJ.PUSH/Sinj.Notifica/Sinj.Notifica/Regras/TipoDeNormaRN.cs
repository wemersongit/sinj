using System.Collections.Generic;
using System.Linq;
using Sinj.Notifica.AcessaDados;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.Regras
{
    public class TipoDeNormaRN
    {
        private static List<TipoDeNorma> _listaTiposDeNorma;
        private TipoDeNormaAD _tipoDeNormaAd;

        public TipoDeNormaRN(string stringConnection)
        {
            _tipoDeNormaAd = new TipoDeNormaAD(stringConnection);
        }

        public List<TipoDeNorma> BuscaTodosTiposDeNorma()
        {
            if (_listaTiposDeNorma == null || _listaTiposDeNorma.Count < 1)
            {
                _listaTiposDeNorma = _tipoDeNormaAd.BuscaTiposDeNorma();
            }
            return _listaTiposDeNorma;
        }

        public TipoDeNorma BuscaTipoDeNorma(string id)
        {
            if (_listaTiposDeNorma == null || _listaTiposDeNorma.Count < 1)
            {
                BuscaTodosTiposDeNorma();
            }
            TipoDeNorma tipoDeNorma = (from t in _listaTiposDeNorma where t.Id.ToString() == id select t).First();
            return tipoDeNorma;
        }
    }
}