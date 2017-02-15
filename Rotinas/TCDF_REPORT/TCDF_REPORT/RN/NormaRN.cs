using TCDF_REPORT.AD;
using TCDF_REPORT.OV;

namespace TCDF_REPORT.RN
{
    public class NormaRN
    {
        private NormaAD _ad;
        public NormaRN(string stringConnection)
        {
            _ad = new NormaAD(stringConnection);
        }
        public int ContarNormasQueContemOTermo(TermoOV termoOv)
        {
            
            return _ad.ContarNormasQueContemOTermo(termoOv);
        }
    }
}