using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web
{
    public partial class EditarOrgao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Util.ValidarAcessoNasPaginas(base.Page, AcoesDoUsuario.org_edt);
        }
    }
}
