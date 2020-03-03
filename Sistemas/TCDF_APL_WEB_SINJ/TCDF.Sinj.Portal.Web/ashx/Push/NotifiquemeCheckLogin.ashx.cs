using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for checkLoginPush
    /// </summary>
    public class NotifiquemeCheckLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var notifiquemeRn = new NotifiquemeRN();
            string sRetorno;
            try
            {
                sRetorno = notifiquemeRn.ChecarSessaoAtiva().ToString().ToLower();
            }
            catch
            {
                sRetorno = "false";
            }
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
