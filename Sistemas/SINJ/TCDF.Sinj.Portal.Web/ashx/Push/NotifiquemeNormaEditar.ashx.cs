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
    /// Summary description for NotifiquemeNormaEditar
    /// </summary>
    public class NotifiquemeNormaEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch_norma = context.Request["ch_norma"];
            var _path = context.Request["path"];
            var _value = context.Request["value"];
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = "PORTAL_PUS.EDT";
            try
            {
                if (!string.IsNullOrEmpty(_ch_norma))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    var j = -1;
                    for (var i = 0; i < notifiquemeOv.normas_monitoradas.Count; i++)
                    {
                        if (notifiquemeOv.normas_monitoradas[i].ch_norma_monitorada == _ch_norma)
                        {
                            j = i;
                            break;
                        }
                    }
                    if (j > -1)
                    {
                        var retornoPath = notifiquemeRn.PathPut(id_push, "normas_monitoradas/" + j + "/"+_path, _value, null);
                        if (retornoPath == "UPDATED")
                        {
                            sRetorno = "{\"ch_doc_success\":\"" + _ch_norma + "\"}";
                        }
                        else
                        {
                            throw new Exception("Erro ao alterar status do monitoramento da norma. ch_doc:" + _ch_norma);
                        }
                    }
                }
                else
                {
                    throw new Exception("Erro ao alterar status do monitoramento da norma. ch_doc:" + _ch_norma);
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
                LogErro.gravar_erro(action, erro, "visitante", "visitante");
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