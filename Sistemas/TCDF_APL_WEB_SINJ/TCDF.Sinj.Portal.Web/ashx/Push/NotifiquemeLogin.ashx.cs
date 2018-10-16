using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using neo.BRLightSession;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeLogin
    /// </summary>
    public class NotifiquemeLogin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var notifiquemeRn = new NotifiquemeRN();
            string sRetorno;
            var bSucesso = false;
            var nm_cookie_push = Config.ValorChave("NmCookiePush");
            var nm_cookie_push_look = Config.ValorChave("NmCookiePushLook");
            var _email_usuario_push = context.Request["email_usuario_push"];
            var _senha_usuario_push = context.Request["senha_usuario_push"];
            var _persist = context.Request["persist"];
            SessaoNotifiquemeOV sessao = null;
            try
            {
                if (string.IsNullOrEmpty(_email_usuario_push))
                {
                    sRetorno = "{\"error_message\": \"Login inválido!!!\" }";
                }
                else if (string.IsNullOrEmpty(_email_usuario_push))
                {
                    sRetorno = "{\"error_message\": \"Senha inválida!!!\" }";
                }
                else
                {
                    try
                    {
                        sessao = notifiquemeRn.LerSessaoNotifiquemeOv();
                    }
                    catch
                    {

                    }
                    if (sessao != null && sessao.email_usuario_push == _email_usuario_push)
                    {
                        sRetorno = "{\"login_notifiqueme\": true}";
                        bSucesso = true;
                    }
                    else
                    {
                        if (sessao != null && sessao.email_usuario_push != _email_usuario_push && !string.IsNullOrEmpty(sessao.email_usuario_push))
                        {
                            sRetorno = "{\"error_message\": \"Para iniciar o sistema novamente finalize a sessão que já existe!!!\" }";
                            Cookies.DeleteCookie(nm_cookie_push);
                            Cookies.DeleteCookie(nm_cookie_push_look);
                        }
                        else
                        {
                            if (!Cookies.CookiesSupported)
                            {
                                sRetorno = "{\"error_message\": \"Não foi possível efetuar login!!! Navegador não suporta Cookies!!\" }";
                            }
                            else
                            {
                                var notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                                if (notifiquemeOv != null)
                                {
                                    var password_md5 = Criptografia.CalcularHashMD5(_senha_usuario_push, true);
                                    if (notifiquemeOv.senha_usuario_push == password_md5)
                                    {
                                        sessao = notifiquemeRn.CriarSessao(notifiquemeOv, (_persist == "1"));
                                        if (sessao != null)
                                        {
                                            sRetorno = "{\"login_notifiqueme\": true}";
                                            bSucesso = true;
                                        }
                                        else
                                        {
                                            sRetorno = "{\"error_message\": \"Não foi possível efetuar login!!! Erro ao criar Sessão!!!\" }";
                                        }
                                    }
                                    else
                                    {
                                        sRetorno = "{\"error_message\": \"E-mail ou senha incorretos!!!\" }";
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"error_message\": \"E-mail ou senha incorretos!!!\" }";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is DocNotFoundException)
                {
                    sRetorno = "{\"error_message\": \"Usuário não encontrado. Verifique se o e-mail usado está correto.\"}";
                }
                else
                {
                    var retorno = new
                    {
                        responseText = Excecao.LerInnerException(ex, true),
                        statusText = "Erro interno do Servidor!!!",
                        status = 500,
                        url = context.Request.Url.PathAndQuery,
                        ErroCallBack = true,
                        ShowErrorServer = true
                    };
                    sRetorno = JSON.Serialize<object>(retorno);
                    context.Response.StatusCode = 500;
                }
                Cookies.DeleteCookie(nm_cookie_push);
                Cookies.DeleteCookie(nm_cookie_push_look);
            }

            // deleta sessões que estão expiradas
            try
            {
                new Session().deleteExperi();
            }
            catch
            {

            }
            if (sessao != null)
            {
                LogAcesso.gravar_acesso("SINJ.PUSH", bSucesso, sRetorno, sessao.nm_usuario_push, sessao.email_usuario_push);
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
