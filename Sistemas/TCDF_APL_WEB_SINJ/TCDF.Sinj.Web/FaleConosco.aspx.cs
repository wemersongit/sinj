using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using neo.BRLightREST;

namespace TCDF.Sinj.Web
{
    public partial class FaleConosco : System.Web.UI.Page
    {
        protected string nmOrgaoCadastrador;
        protected ulong totalNovosOrgao;
        protected void Page_Load(object sender, EventArgs e)
        {
            nmOrgaoCadastrador = Sinj.oSessaoUsuario.orgao_cadastrador.nm_orgao_cadastrador;
            totalNovosOrgao = new FaleConoscoRN().Consultar(new Pesquisa() { select = new string[0], literal = "st_atendimento='Novo' AND nm_orgao_cadastrador_atribuido='" + nmOrgaoCadastrador + "'" }).result_count;
        }
    }
}
