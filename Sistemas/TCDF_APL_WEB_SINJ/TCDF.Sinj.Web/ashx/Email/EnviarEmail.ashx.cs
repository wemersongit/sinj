using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Email
{
    /// <summary>
    /// Summary description for EnviarEmail
    /// </summary>
    public class EnviarEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";

            var _emails = context.Request.Params.GetValues("email");
            var _assunto = context.Request["assunto"];
            var _mensagem = context.Request["mensagem"];

            var action = AcoesDoUsuario.aud_pus;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                sRetorno = EnviarEmails(_emails, _assunto, false, _mensagem);

                var logEmail = new LogEmail();
                logEmail.emails = _emails;
                logEmail.assunto = _assunto;
                logEmail.mensagem = _mensagem;

                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".EMAIL", logEmail, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocValidacaoException || ex is SessionExpiredException)
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".EMAIL", erro, sessao_usuario.nm_usuario, sessao_usuario.email_usuario);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
        }

        public string EnviarEmails(string[] emails, string assunto, bool html, string mensagem)
        {
            var sRetorno = "";
            if (emails.Length <= 0)
            {
                throw new DocValidacaoException("Nenhum destinatário selecionado.");
            }
            else if (string.IsNullOrEmpty(assunto))
            {
                throw new DocValidacaoException("Assunto em branco.");
            }
            else if (string.IsNullOrEmpty(mensagem))
            {
                throw new DocValidacaoException("Mensagem em branco.");
            }
            else
            {
                new EmailRN().EnviaEmail("SINJ Notifica", emails, assunto, html, mensagem);
                sRetorno = "{\"success_message\": \"E-mail enviado com sucesso.\"}";
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
