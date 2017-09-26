using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for NormaEditarArquivo
    /// </summary>
    public class NormaEditarArquivo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var action = AcoesDoUsuario.nor_edt;
            string arquivo_text = context.Request["arquivo"];
            arquivo_text = HttpUtility.UrlDecode(arquivo_text);
            string _id_file = context.Request["id_file"];
            string _id_doc = context.Request["id_doc"];
            string path_file = context.Request["path"];
            string filename = context.Request["filename"];

            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                //ulong id_doc = 0;
                //ulong.TryParse(_id_doc, out id_doc);
                var arquivo_bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(arquivo_text);
                //var arquivo_bytes = Convert.FromBase64String(arquivo_text);
                if (filename.IndexOf(".htm") < 0 || filename.IndexOf(".html") < 0)
                {
                    filename += ".html";
                }
                var fileParameter = new FileParameter(arquivo_bytes, filename, "text/html");
                var sRetorno_anexar = new NormaRN().AnexarArquivo(fileParameter);
                var oRetorno_anexar = JSON.Deserializa<ArquivoOV>(sRetorno_anexar);
                if (oRetorno_anexar != null && !string.IsNullOrEmpty(oRetorno_anexar.uuid))
                {
                    //var sucesso = false;
                    //if (id_doc > 0 && !string.IsNullOrEmpty(_id_file))
                    //{
                    //    NormaRN normaRn = new NormaRN();
                    //    var normaOv = new NormaRN().Doc(id_doc);
                    //    if (path_file == "ar_fonte")
                    //    {
                    //        for (var i = 0; i < normaOv.fontes.Count; i++)
                    //        {
                    //            if (normaOv.fontes[i].ar_fonte.id_file == _id_file)
                    //            {
                    //                sucesso = normaRn.PathPut(id_doc, "fontes/" + i + "/ar_fonte", sRetorno_anexar, null) == "UPDATED";
                    //            }
                    //        }
                    //    }
                    //    else if (normaOv.GetType().GetProperty(path_file) != null)
                    //    {
                    //        sucesso = normaRn.PathPut(id_doc, path_file, sRetorno_anexar, null) == "UPDATED";
                    //    }
                    //}
                    //else
                    //{
                    //    sucesso = true;
                    //}
                    //if (sucesso)
                    //{
                    //    sRetorno = "{\"success_message\":\"Arquivo salvo com sucesso.\",\"file\":" + sRetorno_anexar + "}";
                    //    var log_arquivo = new LogUpload
                    //    {
                    //        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                    //    };
                    //    LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".ARQ", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                    //}
                    //else
                    //{
                    //    sRetorno = "{\"error_message\":\"O arquivo não foi salvo.\",\"file\":" + sRetorno_anexar + "}";
                    //}
                    sRetorno = "{\"success_message\":\"Arquivo salvo com sucesso.\",\"file\":" + sRetorno_anexar + "}";
                    var log_arquivo = new LogUpload
                    {
                        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".ARQ", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (ParametroInvalidoException ex)
            {
                sRetorno = "{\"error_message\": \"" + ex.Message + "\" }";
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
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