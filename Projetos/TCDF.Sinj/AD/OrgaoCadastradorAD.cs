using System.Collections.Generic;
using System.Linq;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class OrgaoCadastradorAD
    {
        private AcessoAD<OrgaoCadastradorOV> _acessoAd;
        private static List<OrgaoCadastradorOV> oOrgaosCadastradores;

        public OrgaoCadastradorAD()
        {
            _acessoAd = new AcessoAD<OrgaoCadastradorOV>(util.BRLight.Util.GetVariavel("NmBaseOrgaoCadastrador", true));
        }

        public Results<OrgaoCadastradorOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        public List<OrgaoCadastradorOV> BuscarTodos()
        {
            if(oOrgaosCadastradores == null || oOrgaosCadastradores.Count <= 0)
            {
                Pesquisa query = new Pesquisa();
                query.limit = null;
                oOrgaosCadastradores = Consultar(query).results;
            }
            return oOrgaosCadastradores;
        }

        public OrgaoCadastradorOV Doc(int id_orgao_cadastrador)
        {
            var orgao_cadastrador = BuscarTodos().Where(o => o.id_orgao_cadastrador == id_orgao_cadastrador);
            if(orgao_cadastrador.Count() > 0)
            {
                return orgao_cadastrador.First();
            }
            return null;
        }
    }
}
