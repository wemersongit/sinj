using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web
{
    public partial class Imagens : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = AcoesDoUsuario.arq_pro + ".img";
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
            }
            catch (SessionExpiredException)
            {
                Response.Redirect("./Login.aspx?cd=1", true);
            }
            catch (Exception ex)
            {
                Response.Redirect("./Erro.aspx", true);
                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(action, erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
        }

        public static string jsValorChave()
        {
            return string.Concat(
                "  var _urlPadrao = '", Util._urlPadrao, "';"
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
