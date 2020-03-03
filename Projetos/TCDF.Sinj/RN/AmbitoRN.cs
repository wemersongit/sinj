using System.Collections.Generic;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.RN
{
    public class AmbitoRN
    {
        private AmbitoAD _ambitoAd;

        public AmbitoRN()
        {
            _ambitoAd = new AmbitoAD();
        }

        public AmbitoOV Doc(int id_ambito)
        {
            return _ambitoAd.Doc(id_ambito);
        }

        public List<AmbitoOV> BuscarTodos()
        {
            return _ambitoAd.BuscarTodos();
        }
    }
}
