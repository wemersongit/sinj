using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx
{
    /// <summary>
    /// Summary description for ErrorAjax
    /// </summary>
    public class ErrorAjax : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SessaoUsuarioOV sessao_usuario = null;
            sessao_usuario = Util.ValidarSessao();

            var message = context.Request["message"];
            var _url = context.Request["url"];
            var _pagina = context.Request["pagina"];

            var _erro = new ErroAjax()
            {
                Pagina = _pagina,
                Mensagem = message,
                Url = _url,
            };

            var _json = JSON.Serialize<ErroAjax>(_erro);

            LogErro.gravar_erro("Ajax", _erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

            context.Response.ContentType = "application/javascript";
            context.Response.Write("({});");
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
