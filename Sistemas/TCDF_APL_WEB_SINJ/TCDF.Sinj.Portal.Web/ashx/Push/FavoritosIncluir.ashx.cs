using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for FavoritosIncluir
    /// </summary>
    public class FavoritosIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch = context.Request["ch"];
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = "PORTAL_FAV.EDT";
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (notifiquemeOv.favoritos.Count<string>(n => n == _ch) <= 0)
                    {
                        notifiquemeOv.favoritos.Add(_ch);
                        string sFavoritos = JSON.Serialize<List<string>>(notifiquemeOv.favoritos);
                        var retornoPath = notifiquemeRn.PathPut(id_push, "favoritos", sFavoritos, null);
                        if (retornoPath == "UPDATED")
                        {
                            notifiquemeOv = notifiquemeRn.Doc(id_push);
                            new NotifiquemeRN().AtualizarSessao(notifiquemeOv);
                            sRetorno = "{\"favoritos\":" + sFavoritos + "}";
                        }
                        else
                        {
                            throw new Exception("Erro ao marcar favoritos. ch:" + _ch);
                        }
                    }
                }
                else
                {
                    throw new Exception("Erro ao marcar favoritos. ch:" + _ch);
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
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
                if (sessaoNotifiquemeOv != null)
                {
                    LogErro.gravar_erro(action, erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
                }
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