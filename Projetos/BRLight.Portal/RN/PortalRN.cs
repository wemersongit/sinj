using BRLight.Portal.AD;
using BRLight.Portal.OV;
using neo.BRLightREST;

namespace BRLight.Portal.RN
{
    public class PortalRN
    {
        private PortalAD _portalAd;

        public PortalRN()
        {
            _portalAd = new PortalAD();
        }

        public PortalRN(string nm_base)
        {
            _portalAd = new PortalAD(nm_base);
        }

        public PortalOV BuscarPortal(string nm_portal)
        {
            Pesquisa pesquisa = new Pesquisa();
            pesquisa.offset = "0";
            pesquisa.limit = "1";
            pesquisa.literal = string.Format("nm_portal='{0}'", nm_portal);
            var portais = Consultar(pesquisa);
            PortalOV portalOv = null;
            if(portais.results.Count > 0)
            {
                portalOv = portais.results[0];
            }
            return portalOv;
        }

        public PortalOV ConsultarReg(ulong id_doc)
        {
            return _portalAd.ConsultarReg(id_doc);
        }

        public Results<PortalOV> Consultar(Pesquisa pesquisa)
        {
            return _portalAd.Consultar(pesquisa);
        }
    }
}