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
    /// Summary description for TipoDeEdicaoDetalhes
    /// </summary>
    public class TipoDeEdicaoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_tipo_edicao = context.Request["ch_tipo_edicao"];
            
            ulong id_doc = 0;
            var tipoDeEdicaoRn = new TipoDeEdicaoRN();
            TipoDeEdicaoOV tipoDeEdicaoOv = null;
            var action = AcoesDoUsuario.tdf_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    tipoDeEdicaoOv = tipoDeEdicaoRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_tipo_edicao))
                {
                    tipoDeEdicaoOv = tipoDeEdicaoRn.Doc(_ch_tipo_edicao);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (tipoDeEdicaoOv != null)
                {
                    sRetorno = JSON.Serialize<TipoDeEdicaoOV>(tipoDeEdicaoOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_tipo_edicao
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
