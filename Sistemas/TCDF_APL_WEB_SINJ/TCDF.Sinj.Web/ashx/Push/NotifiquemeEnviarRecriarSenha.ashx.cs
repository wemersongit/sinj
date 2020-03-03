using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeEnviarRecriarSenha
    /// </summary>
    public class NotifiquemeEnviarRecriarSenha : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{\"void\":0}";
            var _email_usuario_push = context.Request["email_usuario_push"];
            var _captcha_send = context.Request["send"];
            var action = AcoesDoUsuario.pus_vis;
            if (_captcha_send == "1")
            {
                NotifiquemeOV notifiquemeOv = null;
                var notifiquemeRn = new NotifiquemeRN();
                try
                {
                    notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                    if (notifiquemeOv != null)
                    {
                        var email = new EmailRN();
                        var display_name_remetente = "SINJ";
                        var destinatario = new[] { _email_usuario_push };
                        var titulo = "Senha do Notifique-me";
                        var html = true;
                        var token = new Token().Criar("sinj", "recriar_senha_push");
                        var corpo = "Foi solicitada uma recriação de senha para sua conta do Notifique-me SINJ.<br/>"+
                            "Para prosseguir clique no link:<br/><a href='" + Util.GetUriAndPath() + "/RecriarSenhaNotifiqueme.aspx?recriar=" + notifiquemeOv._metadata.id_doc + "A" + token + "' target='_blank' title='Recriar Senha Notifique-me'>Recriar Senha</a><br/>"+
                            "Caso desconheça essa solicitação, basta ignorar este e-mail.";
                        email.EnviaEmail(display_name_remetente, destinatario, titulo, html, corpo);
                        sRetorno = "{\"id_doc_success\": "+notifiquemeOv._metadata.id_doc+" }";
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, "", "");
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
