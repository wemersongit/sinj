using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx
{
    /// <summary>
    /// Summary description for ErrorJS
    /// </summary>
    public class ErrorJS : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            //var callback = context.Request["callback"];
            // irá sair da function caso não exista session
            //SessaoUsuario.validaSession(callback);

            var message = context.Request["message"];
            var _url = context.Request["url"];
            var linenumber = context.Request["linenumber"];
            var _pagina = context.Request["pagina"];

            var _erro = new ErroJS()
            {
                Pagina = _pagina,
                Linha = linenumber,
                Mensagem = message,
                Url = _url,
            };

            var _json = JSON.Serialize<ErroJS>(_erro);

            LogErro.gravar_erro("JavaScript", _erro, "PORTAL", "sinj_portal");


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