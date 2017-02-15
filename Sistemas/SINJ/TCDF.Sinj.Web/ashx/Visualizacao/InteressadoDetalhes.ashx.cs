using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for InteressadoDetalhes
    /// </summary>
    public class InteressadoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_interessado = context.Request["ch_interessado"];
            ulong id_doc = 0;
            var interessadoRn = new InteressadoRN();
            InteressadoOV interessadoOv = null;
            var action = AcoesDoUsuario.int_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    interessadoOv = interessadoRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_interessado))
                {
                    interessadoOv = interessadoRn.Doc(_ch_interessado);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (interessadoOv != null)
                {
                    sRetorno = JSON.Serialize<InteressadoOV>(interessadoOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_interessado
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + _id_doc + "}";
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