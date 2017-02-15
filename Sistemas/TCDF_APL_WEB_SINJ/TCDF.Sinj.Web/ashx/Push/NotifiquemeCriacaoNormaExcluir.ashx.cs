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
    /// Summary description for NotifiquemeCriacaoNormaExcluir
    /// </summary>
    public class NotifiquemeCriacaoNormaExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch_criacao_norma_monitorada = context.Request["ch_criacao_norma_monitorada"];

            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = AcoesDoUsuario.pus_edt;
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_criacao_norma_monitorada))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;

                    var criacao_normas_monitoradas = new List<CriacaoDeNormaMonitoradaPushOV>();
                    foreach(var criacao in notifiquemeOv.criacao_normas_monitoradas)
                    {
                        if (criacao.ch_criacao_norma_monitorada == _ch_criacao_norma_monitorada)
                        {
                            continue;
                        }
                        criacao_normas_monitoradas.Add(criacao);
                    }
                    var retornoPath = notifiquemeRn.PathPut(id_push, "criacao_normas_monitoradas", JSON.Serialize<List<CriacaoDeNormaMonitoradaPushOV>>(criacao_normas_monitoradas), null);

                    if (retornoPath == "UPDATED")
                    {
                        sRetorno = "{\"id_doc_success\":\"" + id_push + "\"}";
                    }
                    else
                    {
                        throw new Exception("Erro ao remover critério do monitoramento. id_push:" + id_push);
                    }
                }
                else
                {
                    throw new Exception("Erro ao remover critério do monitoramento. id_push:" + id_push);
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
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