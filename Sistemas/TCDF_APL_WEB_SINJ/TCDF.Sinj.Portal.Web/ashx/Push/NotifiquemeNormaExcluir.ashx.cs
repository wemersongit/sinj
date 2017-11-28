﻿using System;
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
    /// Summary description for NotifiquemeNormaExcluir
    /// </summary>
    public class NotifiquemeNormaExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch_norma = context.Request["ch_norma"];
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = AcoesDoUsuario.pus_edt;
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_norma))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    notifiquemeOv.normas_monitoradas.RemoveAll(n => n.ch_norma_monitorada == _ch_norma);
                    if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                    {
                        new NotifiquemeRN().AtualizarSessao(notifiquemeOv);
                        notifiquemeOv.senha_usuario_push = null;
                        sRetorno = JSON.Serialize<NotifiquemeOV>(notifiquemeOv);
                    }
                    else
                    {
                        throw new Exception("Erro ao remover monitoramento da norma. ch_doc:" + _ch_norma);
                    }
                }
                else
                {
                    throw new Exception("Erro ao remover monitoramento da norma. ch_doc:" + _ch_norma);
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + "NORMA.DEL", erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
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