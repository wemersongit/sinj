using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeEnviarRecriarSenha
    /// </summary>
    public class NotifiquemeEnviarRecriarSenha : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _tipoDeVerificacao = context.Request["tpv"];

            var _email_usuario_push = context.Request["email_usuario_push"];
            var action = "PORTAL_PUS_REC.ENV";

            try
            {
                var notifiquemeRn = new NotifiquemeRN();
                var notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                if (notifiquemeOv != null)
                {

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

                    var email = new EmailRN();
                    var display_name_remetente = "SINJ";
                    var destinatario = new[] { _email_usuario_push };
                    var titulo = "Senha do Notifique-me";
                    var html = true;
                    var token = new Token().Criar("sinj", "recriar_senha_push");
                    var corpo = "Foi solicitada uma recriação de senha para sua conta do Notifique-me SINJ.<br/>" +
                        "Para prosseguir clique no link:<br/><a href='" + TCDF.Sinj.Util.GetUriAndPath() + "/RecriarSenhaNotifiqueme?recriar=" + notifiquemeOv._metadata.id_doc + "A" + token + "' target='_blank' title='Recriar Senha Notifique-me SINJ'>Recriar Senha</a><br/>" +
                        "Caso desconheça essa solicitação, basta ignorar este e-mail.";
                    email.EnviaEmail(display_name_remetente, destinatario, titulo, html, corpo);
                    sRetorno = "{\"id_doc_success\": " + notifiquemeOv._metadata.id_doc + " }";
                }
                else
                {
                    sRetorno = "{\"error_message\": \"Email informado não existe.\" }";
                }
            }
            catch (Exception ex)
            {
                sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                context.Response.StatusCode = 500;
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro(action, erro, "visitante", "visitante");
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
