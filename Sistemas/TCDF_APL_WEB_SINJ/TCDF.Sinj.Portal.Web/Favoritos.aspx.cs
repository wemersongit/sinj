using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCDF.Sinj.Portal.Web
{
    public partial class Favoritos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Util.ehReplica())
            {
                Response.Redirect("./", true);
            }
            try
            {
                var sessaoPush = TCDF.Sinj.Util.ValidarSessaoPush();
                
            }
            catch
            {
                Response.Redirect("./LoginNotifiqueme?p=Favoritos&message=Realize login para visualizar seus favoritos.&type=alert", true);
            }
        }
    }
}