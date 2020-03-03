using System.Web;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Login
{
    /// <summary>
    /// Summary description for checkLogin
    /// </summary>
    public class checkLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sessaoRn = new SessaoRN();
            string sRetorno;
            try
            {
                sRetorno = sessaoRn.ChecarSessaoAtiva().ToString().ToLower();
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
