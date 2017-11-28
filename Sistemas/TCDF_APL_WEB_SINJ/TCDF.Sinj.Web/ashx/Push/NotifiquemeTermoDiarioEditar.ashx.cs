using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeTermoDiarioEditar
    /// </summary>
    public class NotifiquemeTermoDiarioEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _ch_termo_diario_monitorado = context.Request["ch_termo_diario_monitorado"];
            var _ch_tipo_fonte_diario_monitorado = context.Request["ch_tipo_fonte_diario_monitorado"];
            var _nm_tipo_fonte_diario_monitorado = context.Request["nm_tipo_fonte_diario_monitorado"];
            var _ds_termo_diario_monitorado = context.Request["ds_termo_diario_monitorado"];
            var _st_termo_diario_monitorado = context.Request["st_termo_diario_monitorado"];

            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var notifiquemeRn = new NotifiquemeRN();
            var action = AcoesDoUsuario.pus_edt;
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_termo_diario_monitorado) && (!string.IsNullOrEmpty(_ds_termo_diario_monitorado) || !string.IsNullOrEmpty(_st_termo_diario_monitorado)))
                {
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;

                    foreach (var termo_diario_monitorado in notifiquemeOv.termos_diarios_monitorados)
                    {
                        if (termo_diario_monitorado.ch_termo_diario_monitorado == _ch_termo_diario_monitorado)
                        {
                            if (string.IsNullOrEmpty(_st_termo_diario_monitorado))
                            {
                                termo_diario_monitorado.ch_tipo_fonte_diario_monitorado = _ch_tipo_fonte_diario_monitorado;
                                termo_diario_monitorado.nm_tipo_fonte_diario_monitorado = _nm_tipo_fonte_diario_monitorado;
                                termo_diario_monitorado.ds_termo_diario_monitorado = _ds_termo_diario_monitorado;
                            }
                            else
                            {
                                termo_diario_monitorado.st_termo_diario_monitorado = _st_termo_diario_monitorado == "1";
                            }
                            break;
                        }
                    }
                    if (notifiquemeOv.termos_diarios_monitorados.Count<TermoDiarioMonitoradoPushOV>(t => t.ds_termo_diario_monitorado.Equals(_ds_termo_diario_monitorado, StringComparison.InvariantCultureIgnoreCase) && t.ch_termo_diario_monitorado == _ch_tipo_fonte_diario_monitorado) > 1)
                    {
                        throw new DocDuplicateKeyException("Não é possível salvar essa informação porque ela está duplicada.");
                    }
                    if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                    {
                        notifiquemeOv.senha_usuario_push = null;
                        sRetorno = JSON.Serialize<NotifiquemeOV>(notifiquemeOv);
                    }
                    else
                    {
                        throw new Exception("Erro ao editar termo para monitorar diário. id_push:" + id_push);
                    }
                }
                else
                {
                    throw new Exception("Erro ao editar termo para monitorar diário. id_push:" + id_push);
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + "DIARIO.EDT", erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
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