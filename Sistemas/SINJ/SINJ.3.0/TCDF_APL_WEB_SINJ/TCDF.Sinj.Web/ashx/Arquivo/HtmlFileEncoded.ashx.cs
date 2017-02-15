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
                    var docOv = new Doc(_nm_base).doc(_id_file);
                    var sArquivo = GetHtmlFile(_id_file, _nm_base, docOv);
                    sArquivo = HttpUtility.UrlEncodeUnicode(sArquivo).Replace("+", "%20");
                    //sArquivo = HttpUtility.UrlEncode(sArquivo, Encoding.UTF8).Replace("+", "%20");
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

        public string GetHtmlFile(string _id_file, string _nm_base, File docOv)
        {
            var sArquivo = "";
            var docRn = new Doc(_nm_base);
            if (docOv == null)
            {
                docOv = docRn.doc(_id_file);
            }

            if (docOv.id_file != null && docOv.mimetype == "text/html")
            {
                var file = docRn.download(_id_file);
                if (file != null && file.Length > 0)
                {
                    sArquivo = Encoding.UTF8.GetString(file);
                    //o editor de html (ckeditor) coloca o title dento do body autocomaticamente, então as tags e retorno só conteúdo do body, 
                    sArquivo = Regex.Replace(sArquivo, "<html>.*<body>|</body></html>", String.Empty);
                    sArquivo = HttpUtility.HtmlDecode(sArquivo);

                }
                else
                {
                    throw new Exception("Arquivo não encontrado.");
                }
            }
            else
            {
                throw new Exception("Arquivo não encontrado.");
            }
            return sArquivo;
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