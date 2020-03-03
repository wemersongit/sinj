using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class VocabularioControladoRN
    {
        private VocabularioControladoAD _vocabularioControladoAd;

        public VocabularioControladoRN()
        {
            _vocabularioControladoAd = new VocabularioControladoAD();
        }

        public List<VocabularioControladoLBW> BuscarVocabulariosControladosLBW()
        {
            return _vocabularioControladoAd.BuscarVocabulariosControladosLBW();
        }

        public List<string> BuscarVocabulariosControladosLBW2()
        {
            return _vocabularioControladoAd.BuscarVocabularioControladoLBW2();
        }



        public ulong Incluir(VocabularioControladoOV vocabularioControladoOv)
        {
            return _vocabularioControladoAd.Incluir(vocabularioControladoOv);
        }
    }
}
