using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for FaleConoscoDetalhes
    /// </summary>
    public class FaleConoscoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_chamado = context.Request["ch_chamado"];
            ulong id_doc = 0;
            var autoriaRn = new FaleConoscoRN();
            var sAction = "FLC.VIS";
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                if (!string.IsNullOrEmpty(_ch_chamado))
                {
                    var faleConoscoOv = autoriaRn.Doc(_ch_chamado);

                    sRetorno = JSON.Serialize<FaleConoscoOV>(faleConoscoOv);

                    var log_visualizar = new LogVisualizar
                    {
                        id_doc = id_doc,
                        ch_doc = _ch_chamado
                    };
                    LogOperacao.gravar_operacao(sAction, log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + _ch_chamado + "}";
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
                    LogErro.gravar_erro(sAction, erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            context.Response.ContentType = "application/json";
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