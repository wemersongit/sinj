using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCDF.Sinj.Web
{
    public partial class Notifiqueme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Util.ValidarSessaoPush();
            }
            catch
            {
                Response.Redirect("./LoginNotifiqueme.aspx",true);
            }
        }
    }
}
