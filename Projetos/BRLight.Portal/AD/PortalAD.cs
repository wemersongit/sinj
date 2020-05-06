using BRLight.Portal.OV;
using neo.BRLightREST;

namespace BRLight.Portal.AD
{
    public class PortalAD
    {
        private string nm_base;

        public PortalAD()
        {
            nm_base = "novo_portal";
        }
        public PortalAD(string nm_base)
        {
            this.nm_base = nm_base;
        }

        public PortalOV ConsultarReg(ulong id_doc)
        {
            return new AcessoAD<PortalOV>(nm_base).ConsultarReg(id_doc);
        }

        public Results<PortalOV> Consultar(Pesquisa opesquisa)
        {
            return new AcessoAD<PortalOV>(nm_base).Consultar(opesquisa);
        }
    }
}