using System.Collections.Generic;
using System.Linq;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class AmbitoAD
    {
        private AcessoAD<AmbitoOV> _acessoAd;
        private static List<AmbitoOV> ambitos;

        public AmbitoAD()
        {
            _acessoAd = new AcessoAD<AmbitoOV>(util.BRLight.Util.GetVariavel("NmBaseAmbito",true));
        }

        public Results<AmbitoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        public List<AmbitoOV> BuscarTodos()
        {
            if(ambitos == null || ambitos.Count <= 0)
            {
                Pesquisa query = new Pesquisa();
                query.limit = null;
                ambitos = Consultar(query).results;
            }
            return ambitos;
        }

        public AmbitoOV Doc(int id_ambito)
        {
            var ambitos = BuscarTodos().Where(a => a.id_ambito == id_ambito);
            if(ambitos.Count() > 0)
            {
                return ambitos.First();
            }
            return null;
        }
    }
}
