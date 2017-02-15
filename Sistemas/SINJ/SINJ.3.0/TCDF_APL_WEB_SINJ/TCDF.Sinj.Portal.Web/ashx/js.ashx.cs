using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx
{
    /// <summary>
    /// Summary description for js
    /// </summary>
    public class js : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var notifiquemeRn = new NotifiquemeRN();
            SessaoNotifiquemeOV oSessaoNotifiqueme;
            try
            {
                oSessaoNotifiqueme = notifiquemeRn.LerSessaoNotifiquemeOv();
            }
            catch
            {
                oSessaoNotifiqueme = null;
            }

            var sRetorno = string.Concat(
                  "  var _urlApps = '", Config.ValorChave("apps", true), "';"
                , "  var _urlPadrao = '", Config.ValorChave("Padrao", true), "';"
                , "  var _ambiente = '", Config.ValorChave("Ambiente"), "';"
                , "  var _versao = '", Config.ValorChave("Versao"), "';"
                , "  var _extensoes = ", Config.ValorChave("Extensoes"), ";"
                , "  var _ambitos = ", JSON.Serialize<List<AmbitoOV>>(new AmbitoRN().BuscarTodos()), ";"
                , "  var _orgaos_cadastradores = ", JSON.Serialize<List<OrgaoCadastradorOV>>(new OrgaoCadastradorRN().BuscarTodos()), ";"
                , "  var _notifiqueme = ", (oSessaoNotifiqueme != null) ? JSON.Serialize<SessaoNotifiquemeOV>(oSessaoNotifiqueme) : "null", ";"
                , "  try { "
                , "     if (!(((typeof (JSON) !== 'undefined') && (typeof (JSON.stringify) === 'function') && (typeof (JSON.parse) === 'function'))) || (/MSIE [567]/.test(navigator.userAgent))) {"
                , "        var s = document.createElement('script');"
                , "        s.type = 'text/javascript';"
                , "        s.async = true;"
                , "        s.src = '", Config.ValorChave("Padrao", true), "/Scripts/json3.min.js' ;"
                , "        document.getElementsByTagName('head')[0].appendChild(s);"
                , "     } "
                , "  } catch (e) { "
                , "     top.document.location.href = '/errorjson.html';"
                , "  } "
                );

            context.Response.Clear();
            context.Response.ContentType = "application/javascript";
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