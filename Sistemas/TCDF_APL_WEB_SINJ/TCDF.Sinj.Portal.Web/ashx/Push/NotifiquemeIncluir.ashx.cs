using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeIncluir
    /// </summary>
    public class NotifiquemeIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _tipoDeVerificacao = context.Request["tpv"];

            var action = "PORTAL_PUS.INC";
            SessaoNotifiquemeOV sessao = null;
            try
            {
                

                var _nm_usuario_push = context.Request["nm_usuario_push"];
                var _email_usuario_push = context.Request["email_usuario_push"];
                var _senha_usuario_push = context.Request["senha_usuario_push"];

                NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
                var email_usuario_push = _email_usuario_push.Split(',');
                var senha_usuario_push = _senha_usuario_push.Split(',');
                if (string.IsNullOrEmpty(_nm_usuario_push))
                {
                    throw new DocValidacaoException("Nome Inválido. Preencha o campo 'Nome'.");
                }
                if (email_usuario_push.Length != 2 || email_usuario_push[0] != email_usuario_push[1])
                {
                    throw new DocValidacaoException("E-mail Inválido. Confirme o E-mail");
                }
                if (senha_usuario_push.Length != 2 || senha_usuario_push[0] != senha_usuario_push[1])
                {
                    throw new DocValidacaoException("Senha Inválida. Confirme a Senha");
                }

                if (_tipoDeVerificacao.Equals("g"))
                {
                    var _gRecaptchaResponse = context.Request["g-recaptcha-response"];
                    new ValidaCaptcha().ValidarCaptchaGoogle(_gRecaptchaResponse);
                }
                else
                {
                    var _ds_captcha = context.Request["ds_captcha"];
                    var _k = context.Request["k"];
                    new ValidaCaptcha().ValidarCaptcha(_ds_captcha, _k);
                }

                notifiquemeOv.nm_usuario_push = _nm_usuario_push;
                notifiquemeOv.email_usuario_push = email_usuario_push[0];
                notifiquemeOv.senha_usuario_push = Criptografia.CalcularHashMD5(senha_usuario_push[0], true);

                var id_doc = new NotifiquemeRN().Incluir(notifiquemeOv);
                if (id_doc > 0)
                {
                    sessao = new NotifiquemeRN().CriarSessao(notifiquemeOv, false);
                    if (sessao != null)
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                    }

                }
                else
                {
                    throw new Exception("Erro ao criar conta do Notifique-me.");
                }

            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
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
                if (sessao != null)
                {
                    LogErro.gravar_erro(action, erro, sessao.nm_usuario_push, sessao.email_usuario_push);
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
