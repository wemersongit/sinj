using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.OV;
using MigradorSINJ.AD;

namespace MigradorSINJ.RN
{
    public class OrgaoRN
    {
        private OrgaoAD _orgaoAd;

        public OrgaoRN()
        {
            _orgaoAd = new OrgaoAD();
        }

        public List<OrgaoLBW> BuscarOrgaosLBW()
        {
            return _orgaoAd.BuscarOrgaosLBW();
        }

        public ulong Incluir(OrgaoOV orgaoOv)
        {
            return _orgaoAd.Incluir(orgaoOv);
        }
    }
}
