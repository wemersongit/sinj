using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.RN
{
    public class SituacaoDeNormaRN
    {
        private SituacaoDeNormaAD _situacaoDeNormaAd;

        public SituacaoDeNormaRN()
        {
            _situacaoDeNormaAd = new SituacaoDeNormaAD();
        }

        public Results<SituacaoDeNormaOV> Consultar(Pesquisa query)
        {
            return _situacaoDeNormaAd.Consultar(query);
        }
    }
}
