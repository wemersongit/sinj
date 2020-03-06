using System.Collections.Generic;
using TCDF_REPORT.AD;
using TCDF_REPORT.OV;

namespace TCDF_REPORT.RN
{
    public class TermoRN
    {
        private TermoAD _ad;
        public TermoRN(string stringConnection)
        {
            _ad = new TermoAD(stringConnection);
        }
        public List<TermoOV> BuscarTodosOsTermos()
        {
            return _ad.BuscarTodosOsTermos();
        }
        public int DeletarTermoNaoUsado(string id_termo)
        {
            return _ad.DeletarTermo(id_termo);
        }

        public TermoOV BuscarTermoPorId(string id_termo)
        {
            return _ad.BuscarTermoPorId(id_termo);
        }
    }
}