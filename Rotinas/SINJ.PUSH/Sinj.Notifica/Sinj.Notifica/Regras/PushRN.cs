using System.Collections.Generic;
using Sinj.Notifica.AcessaDados;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.Regras
{
    public class PushRN
    {
        private PushAD _pushAd;

        public PushRN(string stringConection)
        {
            _pushAd = new PushAD(stringConection);
        }

        public List<Push> BuscaAtivosPush()
        {
            List<Push> lista = _pushAd.BuscaAtivosPush();
            return lista;
        }
    }
}