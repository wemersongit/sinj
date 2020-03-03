using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System.Text;
using util.BRLight;
using TCDF.Sinj.Log;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for HtmlFileEncoded
    /// </summary>
    public class HtmlFileEncoded : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(GetHtmlFileEncoded(context));
            context.Response.End();
        }

        public string GetHtmlFileEncoded(HttpContext context)
        {
            var _id_file = context.Request["id_file"];
            var _nm_base = context.Request["nm_base"];
            var sRetorno = "";

            SessaoUsuarioOV sessao_usuario = null;
            var action = AcoesDoUsuario.arq_pro;

            try
            {
                if (!string.IsNullOrEmpty(_id_file))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.rejeitarInject(_id_file);
                    var docOv = new Doc(_nm_base).doc(_id_file);
                    var sArquivo = new UtilArquivoHtml().GetHtmlFile(_id_file, _nm_base, docOv);
                    sArquivo = HttpUtility.UrlEncodeUnicode(sArquivo).Replace("+", "%20");
                    sRetorno = "{\"file\":" + JSON.Serialize<File>(docOv) + ",\"fileencoded\":\"" + sArquivo + "\"}";
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException)
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".HTML.VIS", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            return sRetorno;
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
