using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for ConfiguracaoDetalhes
    /// </summary>
    public class ConfiguracaoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_usuario = context.Request["ch_usuario"];
            ulong id_doc = 0;
            var usuarioRn = new UsuarioRN();
            object configuracaoOv = null;
            var action = AcoesDoUsuario.cfg_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                UsuarioOV usuarioOv = null;
                if (!string.IsNullOrEmpty(_ch_usuario))
                {
                    usuarioOv = usuarioRn.Doc(_ch_usuario);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (usuarioOv != null)
                {
                    id_doc = usuarioOv._metadata.id_doc;
                    configuracaoOv = new { usuarioOv.nm_login_usuario, usuarioOv.nm_usuario, usuarioOv.email_usuario, usuarioOv.ds_pagina_inicial, usuarioOv.ch_tema, usuarioOv.grupos };
                    sRetorno = JSON.Serialize<object>(configuracaoOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_usuario
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
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
