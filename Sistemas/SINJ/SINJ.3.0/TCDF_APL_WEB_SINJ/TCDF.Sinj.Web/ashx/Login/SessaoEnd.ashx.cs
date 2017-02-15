using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Login
{
    /// <summary>
    /// Summary description for SessaoEnd
    /// </summary>
    public class SessaoEnd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var sessaoRn = new SessaoRN();
            var action = AcoesDoUsuario.usr_sai;
            try
            {
                var nm_cookie = Config.ValorChave("NmCookie");
                var nm_cookie_look = Config.ValorChave("NmCookieLook");
                try
                {
                    // Inicio Registra Log
                    var oSessaoUsuario = sessaoRn.LerSessaoUsuarioOv();
                    var log_sair = new LogSair{
                        id_doc = oSessaoUsuario.id_doc,
                        usuario = oSessaoUsuario.nm_login_usuario,
                        sessao_id = oSessaoUsuario.sessao_id,
                        sessao_chave = oSessaoUsuario.sessao_chave
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_sair, oSessaoUsuario.nm_usuario, oSessaoUsuario.nm_login_usuario);
                    // Fim Registra Log
                }
                catch
                {
                    // TODO: erro no registro de log ver o que fazer!!!
                }

                sessaoRn.Finalizar();
                Cookies.DeleteCookie(nm_cookie);
                Cookies.DeleteCookie(nm_cookie_look);
                sRetorno = "{\"excluido\": true}";
            }
            catch (ParametroInvalidoException ex)
            {
                sRetorno = "{\"error_message\": \"" + ex.Message + "\" }";
            }
            catch (Exception Ex)
            {
                sRetorno = Excecao.LerInnerException(Ex, false);
                context.Response.StatusCode = 500;
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