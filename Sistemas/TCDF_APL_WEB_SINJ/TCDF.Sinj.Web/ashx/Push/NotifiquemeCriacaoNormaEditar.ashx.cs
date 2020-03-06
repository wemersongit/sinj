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
    /// Summary description for NotifiquemeCriacaoNormaEditar
    /// </summary>
    public class NotifiquemeCriacaoNormaEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _ch_criacao_norma_monitorada = context.Request["ch_criacao_norma_monitorada"];
            var _ch_tipo_norma = context.Request["ch_tipo_norma_criacao"];
            var _nm_tipo_norma = context.Request["nm_tipo_norma_criacao"];
            var _primeiro_conector = context.Request["primeiro_conector_criacao"];
            var _ch_orgao = context.Request["ch_orgao_criacao"];
            var _nm_orgao = context.Request["nm_orgao_criacao"];
            var _segundo_conector = context.Request["segundo_conector_criacao"];
            var _ch_termo = context.Request["ch_termo_criacao"];
            var _ch_tipo_termo = context.Request["ch_tipo_termo_criacao"];
            var _nm_termo = context.Request["nm_termo_criacao"];

            var _st_criacao = context.Request["st_criacao"];

            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var notifiquemeRn = new NotifiquemeRN();
            var action = AcoesDoUsuario.pus_edt;
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!String.IsNullOrEmpty(_ch_criacao_norma_monitorada))
                {
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (!String.IsNullOrEmpty(_ch_tipo_norma) || !String.IsNullOrEmpty(_ch_orgao) || !String.IsNullOrEmpty(_ch_termo) || !String.IsNullOrEmpty(_st_criacao))
                    {

                        foreach (var criacao_norma_monitorada in notifiquemeOv.criacao_normas_monitoradas)
                        {
                            if (criacao_norma_monitorada.ch_criacao_norma_monitorada == _ch_criacao_norma_monitorada)
                            {
                                if (string.IsNullOrEmpty(_st_criacao))
                                {
                                    criacao_norma_monitorada.ch_tipo_norma_criacao = _ch_tipo_norma;
                                    criacao_norma_monitorada.nm_tipo_norma_criacao = _nm_tipo_norma;
                                    criacao_norma_monitorada.primeiro_conector_criacao = _primeiro_conector;
                                    criacao_norma_monitorada.ch_orgao_criacao = _ch_orgao;
                                    criacao_norma_monitorada.nm_orgao_criacao = _nm_orgao;
                                    criacao_norma_monitorada.segundo_conector_criacao = _segundo_conector;
                                    criacao_norma_monitorada.ch_termo_criacao = _ch_termo;
                                    criacao_norma_monitorada.ch_tipo_termo_criacao = _ch_tipo_termo;
                                    criacao_norma_monitorada.nm_termo_criacao = _nm_termo;
                                }
                                else
                                {
                                    criacao_norma_monitorada.st_criacao = _st_criacao == "1";
                                }
                                
                                break;
                            }
                        }

                        if (notifiquemeOv.criacao_normas_monitoradas.Count<CriacaoDeNormaMonitoradaPushOV>(c => c.ch_orgao_criacao == _ch_orgao && c.ch_termo_criacao == _ch_termo && c.ch_tipo_norma_criacao == _ch_tipo_norma) > 1)
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
                            throw new Exception("Erro ao editar critério para monitorar. id_push:" + id_push);
                        }
                    }
                    else
                    {
                        throw new Exception("Erro ao salvar critério para monitorar. Deve ser informado algum valor para incluí-lo na sua lista de monitoramento.");
                    }
                }
                else
                {
                    throw new Exception("Erro ao editar critério do monitoramento. id_push:" + id_push);
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + "NORMA.CRI.EDT", erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
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
