using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for SessaoExcluir
    /// </summary>
    public class SessaoExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _app = context.Request["app"];
            ulong id_doc = 0;
            var action = AcoesDoUsuario.aud_ses;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    
                    if (new SessaoRN().Excluir(id_doc))
                    {
                        sRetorno = "{\"success_message\":\"Sessão encerrada com sucesso.\"}";
                        var log_excluir = new LogExcluir
                        {
                            id_doc = id_doc,
                            nm_base = "sessao"
                        };
                        LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".EXC", log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                    }
                }
                else if(!string.IsNullOrEmpty(_app)){
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var sessoes_encerradas = new SessaoRN().Excluir(_app == "push" ? "pushlogin" : "usuariologin");
                    if (sessoes_encerradas > 0)
                    {
                        sRetorno = "{\"success_message\":\"" + sessoes_encerradas + " sessões foram encerradas com sucesso.\"}";
                        var log_excluir = new LogExcluir
                        {
                            id_doc = id_doc,
                            nm_base = "sessao" + (_app == "push" ? " de push(todas)" : " de sistema(todas)")
                        };
                        LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".EXC", log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                    }
                    else
                    {
                        sRetorno = "{\"error_message\":\"Nenhuma sessão foi encerrada.\"}";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDependenciesException || ex is SessionExpiredException)
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action)+".EXC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
