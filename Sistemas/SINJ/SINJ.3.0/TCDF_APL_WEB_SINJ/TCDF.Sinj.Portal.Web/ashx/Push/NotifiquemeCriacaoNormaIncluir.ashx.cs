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
    /// Summary description for NotifiquemeCriacaoNormaIncluir
    /// </summary>
    public class NotifiquemeCriacaoNormaIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch_tipo_norma = context.Request["ch_tipo_norma"];
            var _nm_tipo_norma = context.Request["nm_tipo_norma"];
            var _primeiro_conector = context.Request["primeiro_conector"];
            var _ch_orgao = context.Request["ch_orgao"];
            var _nm_orgao = context.Request["nm_orgao"];
            var _segundo_conector = context.Request["segundo_conector"];
            var _ch_termo = context.Request["ch_termo"];
            var _ch_tipo_termo = context.Request["ch_tipo_termo"];
            var _nm_termo = context.Request["nm_termo"];
            var _st_criacao = context.Request["st_criacao"];
            
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = "PORTAL_PUS.EDT";

            var conectores = 0;
            var parametros = 0;

            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_tipo_norma) || !string.IsNullOrEmpty(_ch_orgao) || !string.IsNullOrEmpty(_ch_termo))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;

                    // A diferença de parametros e conectores deve sempre ser igual a 1.
                    if (!string.IsNullOrEmpty(_primeiro_conector))
                    {
                        conectores++;
                    }
                    if (!string.IsNullOrEmpty(_segundo_conector))
                    {
                        conectores++;
                    }
                    
                    if (!string.IsNullOrEmpty(_ch_tipo_norma))
                    {
                        parametros++;
                    }
                    if (!string.IsNullOrEmpty(_ch_orgao))
                    {
                        parametros++;
                    }
                    if (!string.IsNullOrEmpty(_ch_termo))
                    {
                        parametros++;
                    }
                    if ((parametros - conectores) != 1)
                    {
                        throw new Exception("Erro ao adicionar critério para monitorar. id_push:" + id_push);
                    }

                    var criacao_norma_monitorada_ov = new CriacaoDeNormaMonitoradaPushOV
                    {
                        ch_criacao_norma_monitorada = Guid.NewGuid().ToString("N"),
                        ch_tipo_norma_criacao = _ch_tipo_norma,
                        nm_tipo_norma_criacao = _nm_tipo_norma,
                        primeiro_conector_criacao = _primeiro_conector,
                        ch_orgao_criacao = _ch_orgao,
                        nm_orgao_criacao = _nm_orgao,
                        segundo_conector_criacao = _segundo_conector,
                        ch_termo_criacao = _ch_termo,
                        ch_tipo_termo_criacao = _ch_tipo_termo,
                        nm_termo_criacao = _nm_termo,
                        st_criacao = bool.Parse(_st_criacao)
                    };
                    if ( notifiquemeOv.criacao_normas_monitoradas.Count<CriacaoDeNormaMonitoradaPushOV>(c => c.ch_orgao_criacao == criacao_norma_monitorada_ov.ch_orgao_criacao && c.ch_termo_criacao == criacao_norma_monitorada_ov.ch_termo_criacao && c.ch_tipo_norma_criacao == criacao_norma_monitorada_ov.ch_tipo_norma_criacao) <= 0)
                    {
                        notifiquemeOv.criacao_normas_monitoradas.Add(criacao_norma_monitorada_ov);
                        var json_criacao_normas_monitoradas = JSON.Serialize<List<CriacaoDeNormaMonitoradaPushOV>>(notifiquemeOv.criacao_normas_monitoradas);
                        var retornoPath = notifiquemeRn.PathPut(id_push, "criacao_normas_monitoradas", json_criacao_normas_monitoradas, null);
                        if (retornoPath == "UPDATED")
                        {
                            sRetorno = "{\"criacao_normas_monitoradas\":" + json_criacao_normas_monitoradas + "}";
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar critério para monitorar. id_push:" + id_push);
                        }
                    }
                    else
                    {
                        throw new DocDuplicateKeyException("Critério de monitoramento repetido.");
                    }
                }
                else
                {
                    throw new Exception("Erro ao adicionar critério para monitorar. id_push:" + id_push);
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