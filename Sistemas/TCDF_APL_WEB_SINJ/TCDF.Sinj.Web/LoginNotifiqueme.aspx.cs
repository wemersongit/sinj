using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.Web
{
    public partial class LoginNotifiqueme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			var _redirecionar_notifiqueme = Request ["redirecionar_notifiqueme"];

            var notifiquemeRn = new NotifiquemeRN();
            var sessao = notifiquemeRn.LerSessao();
            NotifiquemeOV notifiquemeOv = null;
            if (sessao != null)
            {
                notifiquemeOv = JSON.Deserializa<NotifiquemeOV>(sessao.ds_valor);
            }
			if (String.IsNullOrEmpty(_redirecionar_notifiqueme)){
				if (notifiquemeOv != null && !string.IsNullOrEmpty(notifiquemeOv.email_usuario_push))
				{
					Response.Redirect ("./Notifiqueme.aspx");
				}
			}
			else{
				if (notifiquemeOv != null && !string.IsNullOrEmpty(notifiquemeOv.email_usuario_push))
				{
					Response.Redirect (_redirecionar_notifiqueme);
				}
			}
        }
    }
}
