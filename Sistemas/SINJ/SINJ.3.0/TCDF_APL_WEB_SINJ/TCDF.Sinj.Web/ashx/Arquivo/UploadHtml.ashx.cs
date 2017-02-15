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
                sRetorno = AnexarHtml(_arquivo_text, _filename, _nm_base);
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

        public string AnexarHtml(string _arquivo_text,string _filename, string _nm_base)
        {
            string sRetorno = "";

            SessaoUsuarioOV sessao_usuario = Util.ValidarSessao();

            if (_filename.IndexOf(".htm") < 0 || _filename.IndexOf(".html") < 0)
            {
                _filename += ".html";
            }

            if (_arquivo_text.IndexOf("<head>") < 0)
            {
                _arquivo_text = "<html><head><title>" + _filename.Replace(".html", "") + "</title></head><body>" + _arquivo_text + "</body></html>";
            }

            var arquivo_bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(_arquivo_text);


            var fileParameter = new FileParameter(arquivo_bytes, _filename, "text/html");

            try
            {
                var doc = new Doc(_nm_base);
                var dicionario = new Dictionary<string, object>();
                dicionario.Add("file", fileParameter);
                sRetorno = doc.incluir(dicionario);
                var log_arquivo = new LogUpload
                {
                    arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                };
                LogOperacao.gravar_operacao("HTML.INC", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
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