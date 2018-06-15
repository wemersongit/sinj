using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web
{
    public partial class FaleConosco : System.Web.UI.Page
    {
        protected string sChamados;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Util.ehReplica())
            {
                Response.Redirect("./", true);
            }
            try
            {
                var sessaoPush = TCDF.Sinj.Util.ValidarSessaoPush();
                try
                {
                    var chamados = new FaleConoscoRN().Consultar(new Pesquisa() { limit = null, literal = "ds_email='" + sessaoPush.email_usuario_push + "'" }).results;
                    sChamados = JSON.Serialize<System.Collections.Generic.List<FaleConoscoOV>>(chamados);
                }
                catch
                {
                    sChamados = "[]";
                }
            }
            catch
            {
                Response.Redirect("./LoginNotifiqueme?p=FaleConosco&message=Realize login para visualizar as conversas em 'Fale Conosco'.&type=alert", true);
            }
            
        }
    }
}