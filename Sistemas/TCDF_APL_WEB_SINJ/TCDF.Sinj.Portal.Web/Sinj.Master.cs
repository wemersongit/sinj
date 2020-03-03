using System;
using System.Collections.Generic;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web
{
    public partial class Sinj : System.Web.UI.MasterPage
    {
        private static SessaoNotifiquemeOV oSessaoNotifiqueme;
        protected override void OnInit(EventArgs e)
        {
            var notifiquemeRn = new NotifiquemeRN();
            try
            {
                oSessaoNotifiqueme = notifiquemeRn.LerSessaoNotifiquemeOv();
            }
            catch
            {
                oSessaoNotifiqueme = null;
            }
        }
        public static string jsValorChave() {
            var ambitos = Util.GetAmbitos();
            var orgaosCadastradores = Util.GetOrgaosCadastradores();
            return string.Concat(
                "  var _urlApps = '", util.BRLight.Util.GetVariavel("apps", true), "';"
                , "  var _urlPadrao = '", Util._urlPadrao, "';"
                , "  var _ambiente = '", util.BRLight.Util.GetVariavel("Ambiente"), "';"
                , "  var _aplicacao = '", util.BRLight.Util.GetVariavel("Aplicacao"), "';" //Util quando as aplicações estiverem unificadas...
                , "  var _versao = '", Util.MostrarVersao(), "';"
                , "  var _extensoes = ", util.BRLight.Util.GetVariavel("Extensoes"), ";"
                , "  var _ambitos = ", (!string.IsNullOrEmpty(ambitos) ? ambitos : "[]"), ";"
                , "  var _orgaos_cadastradores = ", (!string.IsNullOrEmpty(orgaosCadastradores) ? ambitos : "[]"), ";"
                , "  var _notifiqueme = ", (oSessaoNotifiqueme != null) ? JSON.Serialize<SessaoNotifiquemeOV>(oSessaoNotifiqueme) : "null", ";"
                , "  var _nm_cookie_push = '", util.BRLight.Util.GetVariavel("NmCookiePush"), "';"
                , "  try { "
                , "     if (!(((typeof (JSON) !== 'undefined') && (typeof (JSON.stringify) === 'function') && (typeof (JSON.parse) === 'function'))) || (/MSIE [567]/.test(navigator.userAgent))) {"
                , "        var s = document.createElement('script');"
                , "        s.type = 'text/javascript';"
                , "        s.async = true;"
                , "        s.src = '", Util._urlPadrao, "/Scripts/json3.min.js' ;"
                , "        document.getElementsByTagName('head')[0].appendChild(s);"
                , "     } "
                , "  } catch (e) { "
                , "     top.document.location.href = '/errorjson.html';"
                , "  } "
                );
        }
    }
}
