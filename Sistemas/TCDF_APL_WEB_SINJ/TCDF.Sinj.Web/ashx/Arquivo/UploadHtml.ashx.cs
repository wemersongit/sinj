using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for HtmlIncluir
    /// </summary>
    public class UploadHtml : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(AnexarHtml(context));
            context.Response.End();
        }

        public string AnexarHtml(HttpContext context)
        {
            string sRetorno = "";
            string _arquivo_text = context.Request["arquivo"];
            _arquivo_text = HttpUtility.UrlDecode(_arquivo_text);
            string _filename = context.Request["nm_arquivo"];
            string _nm_base = context.Request["nm_base"];

            SessaoUsuarioOV sessao_usuario = null;

            try
            {
                sessao_usuario = Util.ValidarSessao();
                sRetorno = new UtilArquivoHtml().AnexarHtml(_arquivo_text, _filename, _nm_base);
                var log_arquivo = new LogUpload
                {
                    arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                };
                LogOperacao.gravar_operacao("HTML.INC", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (ParametroInvalidoException ex)
            {
                sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
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
                    LogErro.gravar_erro("HTML.INC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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