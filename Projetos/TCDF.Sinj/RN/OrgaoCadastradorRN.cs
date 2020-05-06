using System.Collections.Generic;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.RN
{
    public class OrgaoCadastradorRN
    {
        private OrgaoCadastradorAD _orgaoCadastradorAd;

        public OrgaoCadastradorRN()
        {
            _orgaoCadastradorAd = new OrgaoCadastradorAD();
        }

        public OrgaoCadastradorOV Doc(int id_orgao_cadastrador)
        {
            return _orgaoCadastradorAd.Doc(id_orgao_cadastrador);
        }

        public List<OrgaoCadastradorOV> BuscarTodos()
        {
            return _orgaoCadastradorAd.BuscarTodos();
        }
    }
}
