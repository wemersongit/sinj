using System;
using System.Web;
using neo.BRLightREST;
using neo.BRLightSession;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Login
{
    public class Login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var usuarioRn = new UsuarioRN();
            var sessaoRn = new SessaoRN();
            string sRetorno;
            var bSucesso = false;
            var nm_cookie = Config.ValorChave("NmCookie");
            var nm_cookie_look = Config.ValorChave("NmCookieLook");
            var _login = context.Request["login"];
            var _senha = context.Request["senha"];
            var _persist = context.Request["persist"];
            SessaoUsuarioOV sessao = null;
            try
            {
                if (string.IsNullOrEmpty(_login))
                {
                    sRetorno = "{\"error_message\": \"Login inválido!!!\" }";
                }
                else if (string.IsNullOrEmpty(_login))
                {
                    sRetorno = "{\"error_message\": \"Senha inválida!!!\" }";
                }
                else
                {
                    try
                    {
                        sessao = sessaoRn.LerSessaoUsuarioOv();
                    }
                    catch
                    {

                    }
                    if (sessao != null && sessao.nm_login_usuario == _login)
                    {
                        sRetorno = "{\"login\": true}";
                        bSucesso = true;
                    }
                    else
                    {
                        if (sessao != null && sessao.nm_login_usuario != _login && !string.IsNullOrEmpty(sessao.nm_login_usuario))
                        {
                            sessaoRn.Finalizar();
                            Cookies.DeleteCookie(nm_cookie);
                            Cookies.DeleteCookie(nm_cookie_look);
                        }

                        if (!Cookies.CookiesSupported)
                        {
                            sRetorno = "{\"error_message\": \"Não foi possível efetuar login!!! Navegador não suporta Cookies!!\" }";
                        }
                        else
                        {
                            var usuarioOv = usuarioRn.Doc(_login);
                            if (usuarioOv != null && usuarioOv.st_usuario)
                            {
                                var password_md5 = Criptografia.CalcularHashMD5(_senha, true);
                                if (usuarioOv.senha_usuario == password_md5)
                                {
                                    sessao = sessaoRn.CriarSessao(usuarioOv, (_persist == "1"));
                                    if (sessao != null)
                                    {
										sRetorno = "{\"login\": true, \"pagina_inicial\":\""+usuarioOv.pagina_inicial+"\"}";
                                        bSucesso = true;
                                    }
                                    else
                                    {
                                        sRetorno = "{\"error_message\": \"Não foi possível efetuar login!!! Erro ao criar Sessão!!!\" }";
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"error_message\": \"Login ou senha incorretos!!!\" }";
                                }

                            }
                            else
                            {
                                sRetorno = "{\"error_message\": \"Login ou senha incorretos!!!\" }";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
                Cookies.DeleteCookie(nm_cookie);
                Cookies.DeleteCookie(nm_cookie_look);
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
                LogAcesso.gravar_acesso("SINJ.CAD", bSucesso, sRetorno, sessao.nm_usuario, sessao.nm_login_usuario);
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
