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
    /// Summary description for NotifiquemeEditar
    /// </summary>
    public class NotifiquemeEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{\"void\":0}";
            var _nm_usuario_push = context.Request["nm_usuario_push"];
            var _senha_usuario_push_antiga = context.Request["senha_usuario_push_antiga"];
            var _senha_usuario_push = context.Request["senha_usuario_push"];
            var _st_push = context.Request["st_push"];
            var bAtualizado = false;
            ulong id_doc = 0;
            var action = "PORTAL_PUS.EDT";
            NotifiquemeOV notifiquemeOv = null;
            var notifiquemeRn = new NotifiquemeRN();
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                id_doc = notifiquemeOv._metadata.id_doc;
                notifiquemeOv.nm_usuario_push = _nm_usuario_push;
                if (!string.IsNullOrEmpty(_senha_usuario_push_antiga) && !string.IsNullOrEmpty(_senha_usuario_push))
                {
                    var senha_usuario_push = _senha_usuario_push.Split(',');
                    if (senha_usuario_push.Length == 2)
                    {
                        if (senha_usuario_push[0] != senha_usuario_push[1])
                        {
                            throw new DocValidacaoException("Senha Inválida. Confirme a Senha");
                        }
                        var senha_antiga = Criptografia.CalcularHashMD5(_senha_usuario_push_antiga, true);
                        if (notifiquemeOv.senha_usuario_push != senha_antiga)
                        {
                            throw new DocValidacaoException("Senha Incorreta. Necessário informar a senha antiga.");
                        }
                        notifiquemeOv.senha_usuario_push = Criptografia.CalcularHashMD5(senha_usuario_push[0], true);
                        bAtualizado = true;
                    }
                }
                if (!string.IsNullOrEmpty(_nm_usuario_push))
                {
                    notifiquemeOv.nm_usuario_push = _nm_usuario_push;
                    bAtualizado = true;
                }
                if (!string.IsNullOrEmpty(_st_push))
                {
                    notifiquemeOv.st_push = _st_push == "1";
                    bAtualizado = true;
                }
                if (bAtualizado)
                {
                    if (notifiquemeRn.Atualizar(notifiquemeOv._metadata.id_doc, notifiquemeOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + notifiquemeOv._metadata.id_doc + "}";
                    }
                    else
                    {
                        sRetorno = "{\"error_message\": \"Erro ao atualizar conta do Notifique-me.\" }";
                        throw new Exception("Erro ao atualizar conta do Notifique-me.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
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
