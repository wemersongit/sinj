using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    class ExcluidoAD
    {
        private AcessoAD<ExcluidoOV> _acessoAd;

        public ExcluidoAD()
        {
            _acessoAd = new AcessoAD<ExcluidoOV>(util.BRLight.Util.GetVariavel("NmBaseExcluido", true));
        }

        internal ulong Incluir(ExcluidoOV excluidoOv)
        {
            return _acessoAd.Incluir(excluidoOv);
        }
        
        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal bool Excluir(ulong id_doc)
        {
            return _acessoAd.Excluir(id_doc);
        }

        public ExcluidoOV ConsultarReg(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

    }
}
