using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class SituacaoDeNormaAD
    {
        private AcessoAD<SituacaoDeNormaOV> _acessoAd;

        public SituacaoDeNormaAD()
        {
            _acessoAd = new AcessoAD<SituacaoDeNormaOV>(util.BRLight.Util.GetVariavel("NmBaseSituacaoDeNorma", true));
        }

        internal Results<SituacaoDeNormaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }
    }
}