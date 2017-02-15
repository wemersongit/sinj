using System.Collections.Generic;
using System.Linq;
using Sinj.Notifica.AcessaDados;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.Regras
{
    public class OrgaoRN
    {
        private static List<OrgaoSinj> _listaOrgaos;
        private OrgaoAD _orgaoAd;

        public OrgaoRN(string stringConnection)
        {
            _orgaoAd = new OrgaoAD(stringConnection);
        }

        public List<OrgaoSinj> BuscaTodosOrgaos()
        {
            if(_listaOrgaos == null || _listaOrgaos.Count < 1)
            {
                _listaOrgaos = _orgaoAd.BuscaOrgaos();
            }
            return _listaOrgaos;
        }

        public OrgaoSinj BuscaOrgao(string id)
        {
            if (_listaOrgaos == null || _listaOrgaos.Count < 1)
            {
                BuscaTodosOrgaos();
            }
            OrgaoSinj orgaoSinj = (from o in _listaOrgaos where o.Id.ToString() == id select o).First();
            return orgaoSinj;
        }
    }
}