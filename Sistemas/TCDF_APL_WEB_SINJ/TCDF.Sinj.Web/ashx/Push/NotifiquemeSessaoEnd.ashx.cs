using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeSessaoEnd
    /// </summary>
    public class NotifiquemeSessaoEnd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var notifiquemeRn = new NotifiquemeRN();
            var action = AcoesDoUsuario.usr_sai;
            try
            {
                var nm_cookie = Config.ValorChave("NmCookiePush");
                var nm_cookie_look = Config.ValorChave("NmCookiePushLook");
                try
                {
                    // Inicio Registra Log
                    var oSessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    var log_sair = new LogSair
                    {
                        id_doc = oSessaoNotifiquemeOv.id_doc,
                        usuario = oSessaoNotifiquemeOv.email_usuario_push,
                        sessao_id = oSessaoNotifiquemeOv.sessao_id,
                        sessao_chave = oSessaoNotifiquemeOv.sessao_chave
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_sair, oSessaoNotifiquemeOv.nm_usuario_push, oSessaoNotifiquemeOv.email_usuario_push);
                    // Fim Registra Log
                }
                catch
                {
                    // TODO: erro no registro de log ver o que fazer!!!
                }

                notifiquemeRn.Finalizar();
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
