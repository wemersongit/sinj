using System;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web
{
    public partial class CadastrarOrgao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Util.ValidarAcessoNasPaginas(base.Page, AcoesDoUsuario.org_inc);
        }
    }
}