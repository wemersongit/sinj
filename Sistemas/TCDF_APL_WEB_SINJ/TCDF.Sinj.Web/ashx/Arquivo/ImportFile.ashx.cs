using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;
using System.Text;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for ImportFile
    /// </summary>
    public class ImportFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(ImportarArquivo(context));
            context.Response.End();
        }

        public string ImportarArquivo(HttpContext context)
        {
            string sRetorno = "{}";
            var _id_file = context.Request["id_file"];
            var _nm_base_origem = context.Request["nm_base_origem"];
            var _nm_base_destino = context.Request["nm_base_destino"];
            var _ds_diario = context.Request["ds_diario"];
            var action = AcoesDoUsuario.arq_pro;
            var sAction = Util.GetEnumDescription(action) + ".IMP";
            var texto_rodape = "Este texto não substitui o publicado no ";

            SessaoUsuarioOV sessao_usuario = null;

            try
            {
                if (!string.IsNullOrEmpty(_id_file) && !string.IsNullOrEmpty(_nm_base_origem) && !string.IsNullOrEmpty(_nm_base_destino))
                {
                    sAction += "." + _nm_base_origem + "." + _nm_base_destino;
                    sessao_usuario = Util.ValidarSessao();

                    Util.rejeitarInject(_id_file);
                    var docRn = new Doc(_nm_base_origem);
                    var docOv = docRn.doc(_id_file);

                    if (docOv.id_file != null)
                    {
                        var file = docRn.download(_id_file);
                        if (file != null && file.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(_ds_diario) && docOv.mimetype == "text/html")
                            {
                                var sArquivo = Encoding.UTF8.GetString(file);
                                //o editor de html (ckeditor) coloca o title dento do body autocomaticamente, então as tags e retorno só conteúdo do body, 
                                sArquivo = HttpUtility.HtmlDecode(sArquivo);
                                if(sArquivo.IndexOf(texto_rodape) < 0){
                                    var pattern = "</body>";
                                    var replacement = "<p style=\"text-align:right\"><span style=\"color:#FF0000\">"+texto_rodape + _ds_diario + "</span></p></body>";
                                    sArquivo = Regex.Replace(sArquivo, pattern, replacement);
                                    file = System.Text.UnicodeEncoding.UTF8.GetBytes(sArquivo);
                                }
                            }
                            var fileParameter = new FileParameter(file, docOv.filename, docOv.mimetype);
                            try
                            {
                                var doc = new Doc(_nm_base_destino);
                                var dicionario = new Dictionary<string, object>();
                                dicionario.Add("file", fileParameter);
                                sRetorno = doc.incluir(dicionario);
                                
                                var log_arquivo = new LogUpload
                                {
                                    arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                                };
                                LogOperacao.gravar_operacao(sAction, log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

                            }
                            catch (Exception ex)
                            {
                                throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
                            }

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
                    LogErro.gravar_erro(sAction, erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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