using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeIncluir
    /// </summary>
    public class NotifiquemeIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{\"void\":0}";
            var _nm_usuario_push = context.Request["nm_usuario_push"];
            var _email_usuario_push = context.Request["email_usuario_push"];
            var _senha_usuario_push = context.Request["senha_usuario_push"];
            var _captcha_send = context.Request["send"];
            SessaoNotifiquemeOV sessao = null;
            if(_captcha_send == "1"){
                var action = AcoesDoUsuario.pus_inc;
                NotifiquemeOV notifiquemeOv = null;
                try
                {
                    notifiquemeOv = new NotifiquemeOV();
                    var email_usuario_push = _email_usuario_push.Split(',');
                    var senha_usuario_push = _senha_usuario_push.Split(',');
                    notifiquemeOv.nm_usuario_push = _nm_usuario_push;
                    if (email_usuario_push.Length == 2)
                    {
                        if (email_usuario_push[0] != email_usuario_push[1])
                        {
                            throw new DocValidacaoException("E-mail Inválido. Confirme o E-mail");
                        }
                        notifiquemeOv.email_usuario_push = email_usuario_push[0];
                    }
                    if (senha_usuario_push.Length == 2)
                    {
                        if (senha_usuario_push[0] != senha_usuario_push[1])
                        {
                            throw new DocValidacaoException("Senha Inválida. Confirme a Senha");
                        }
                        notifiquemeOv.senha_usuario_push = Criptografia.CalcularHashMD5(senha_usuario_push[0], true);
                    }
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
                        sRetorno = "{\"error_message\": \"Erro ao criar conta do Notifique-me.\" }";
                        throw new Exception("Erro ao criar conta do Notifique-me.");
                    }
                    var log_incluir = new LogIncluir<NotifiquemeOV>
                    {
                        registro = notifiquemeOv
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_incluir, sessao.nm_usuario_push, sessao.email_usuario_push);
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
                        LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao.nm_usuario_push, sessao.email_usuario_push);
                    }
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
