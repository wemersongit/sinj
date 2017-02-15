using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeDetalhes
    /// </summary>
    public class NotifiquemeDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            ulong id_doc = 0;
            var notifiquemeRn = new NotifiquemeRN();
            NotifiquemeOV notifiquemeOv = null;
            try
            {
                var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                id_doc = notifiquemeOv._metadata.id_doc;
                if (notifiquemeOv != null)
                {
                    notifiquemeOv.senha_usuario_push = "";
                    sRetorno = JSON.Serialize<NotifiquemeOV>(notifiquemeOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"Regitro não encontrado.\"}";
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + id_doc + "}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("PORTAL_PUS_VIS", erro, "visitante", "visitante");
            }
            context.Response.ContentType = "application/json";
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