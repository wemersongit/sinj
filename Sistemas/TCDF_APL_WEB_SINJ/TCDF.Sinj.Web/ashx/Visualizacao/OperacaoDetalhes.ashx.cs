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
    /// Summary description for OperacaoDetalhes
    /// </summary>
    public class OperacaoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(DocOperacao(context));
            context.Response.End();
        }

        public string DocOperacao(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var operacaoRn = new Log.RN.log_operacaoRN();
            Log.OV.log_operacaoOV operacaoOv = null;
            var action = AcoesDoUsuario.aud_ope;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    operacaoOv = operacaoRn.ConsultarReg(id_doc);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (operacaoOv != null)
                {
                    sRetorno = JSON.Serialize<Log.OV.log_operacaoOV>(operacaoOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"Operação não encontrada.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".VIS", log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".VIS", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            return sRetorno;
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
