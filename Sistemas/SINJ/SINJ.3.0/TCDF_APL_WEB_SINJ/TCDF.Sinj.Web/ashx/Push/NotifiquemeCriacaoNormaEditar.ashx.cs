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

            var _ch_tipo_norma_novo = context.Request["ch_tipo_norma_novo"];
            var _nm_tipo_norma_novo = context.Request["nm_tipo_norma_novo"];
            var _primeiro_conector_novo = context.Request["primeiro_conector_novo"];
            var _ch_orgao_novo = context.Request["ch_orgao_novo"];
            var _nm_orgao_novo = context.Request["nm_orgao_novo"];
            var _segundo_conector_novo = context.Request["segundo_conector_novo"];
            var _ch_termo_novo = context.Request["ch_termo_novo"];
            var _ch_tipo_termo_novo = context.Request["ch_tipo_termo_novo"];
            var _nm_termo_novo = context.Request["nm_termo_novo"];
            var _st_criacao_novo = context.Request["st_criacao_novo"];

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
                    if (!String.IsNullOrEmpty(_ch_tipo_norma_novo) || !String.IsNullOrEmpty(_ch_orgao_novo) || !String.IsNullOrEmpty(_ch_termo_novo))
                    {
                        var criacao_norma_monitorada_ov_novo = new CriacaoDeNormaMonitoradaPushOV()
                        {
                            ch_tipo_norma_criacao = _ch_tipo_norma_novo,
                            nm_tipo_norma_criacao = _nm_tipo_norma_novo,
                            primeiro_conector_criacao = _primeiro_conector_novo,
                            ch_orgao_criacao = _ch_orgao_novo,
                            nm_orgao_criacao = _nm_orgao_novo,
                            segundo_conector_criacao = _segundo_conector_novo,
                            ch_termo_criacao = _ch_termo_novo,
                            ch_tipo_termo_criacao = _ch_tipo_termo_novo,
                            nm_termo_criacao = _nm_termo_novo,
                            st_criacao = bool.Parse(_st_criacao_novo)
                        };
                        if (notifiquemeOv.criacao_normas_monitoradas.Count<CriacaoDeNormaMonitoradaPushOV>(c => c.ch_criacao_norma_monitorada != _ch_criacao_norma_monitorada && c.ch_orgao_criacao == criacao_norma_monitorada_ov_novo.ch_orgao_criacao && c.ch_termo_criacao == criacao_norma_monitorada_ov_novo.ch_termo_criacao && c.ch_tipo_norma_criacao == criacao_norma_monitorada_ov_novo.ch_tipo_norma_criacao && c.st_criacao == criacao_norma_monitorada_ov_novo.st_criacao) <= 0)
                        {   
                            foreach (var criacao in notifiquemeOv.criacao_normas_monitoradas)
                            {
                                if (criacao.ch_criacao_norma_monitorada == _ch_criacao_norma_monitorada)
                                {
                                    criacao.ch_tipo_norma_criacao = _ch_tipo_norma_novo;
                                    criacao.nm_tipo_norma_criacao = _nm_tipo_norma_novo;
                                    criacao.primeiro_conector_criacao = _primeiro_conector_novo;
                                    criacao.ch_orgao_criacao = _ch_orgao_novo;
                                    criacao.nm_orgao_criacao = _nm_orgao_novo;
                                    criacao.segundo_conector_criacao = _segundo_conector_novo;
                                    criacao.ch_termo_criacao = _ch_termo_novo;
                                    criacao.ch_tipo_termo_criacao = _ch_tipo_termo_novo;
                                    criacao.nm_termo_criacao = _nm_termo_novo;
                                    criacao.st_criacao = bool.Parse(_st_criacao_novo);
                                    break;
                                }
                            }
                            var json_criacao_normas_monitoradas = JSON.Serialize<List<CriacaoDeNormaMonitoradaPushOV>>(notifiquemeOv.criacao_normas_monitoradas);
                            var retornoPath = notifiquemeRn.PathPut(id_push, "criacao_normas_monitoradas", json_criacao_normas_monitoradas, null);
                            if (retornoPath == "UPDATED")
                            {
                                notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                                sRetorno = "{\"criacao_normas_monitoradas\":" + json_criacao_normas_monitoradas + "}";
                            }
                            else
                            {
                                throw new Exception("Erro ao editar critério para monitorar. id_push:" + id_push);
                            }
                        }
                        else
                        {
                            throw new DocDuplicateKeyException("Critério de monitoramento repetido.");
                        }
                    }
                    else
                    {
                        throw new Exception("Erro com novos critérios de monitoramento. id_push:" + id_push);
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