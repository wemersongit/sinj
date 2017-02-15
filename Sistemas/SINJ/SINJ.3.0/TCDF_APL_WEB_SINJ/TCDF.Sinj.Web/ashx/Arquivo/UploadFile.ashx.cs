using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using System.IO;
using neo.BRLightREST;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{}";
            var sAction = "UPL";

            SessaoUsuarioOV sessao_usuario = null;
            HttpPostedFile _file = context.Request.Files["file"];
            var _nm_base = context.Request["nm_base"];

            try
            {
                sAction += "." + _nm_base;
                sessao_usuario = Util.ValidarSessao();
                sRetorno = AnexarArquivo(_nm_base, _file);
                if (sRetorno.IndexOf("id_file") < 0)
                {
                    throw new Exception("Erro ao salvar arquivo.");
                }

                var log_arquivo = new LogUpload
                {
                    arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                };
                LogOperacao.gravar_operacao(sAction, log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

                context.Response.Write(sRetorno);

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
            context.Response.End();
        }

        public string AnexarArquivo(string _nm_base, HttpPostedFile _file)
        {
            string sRetorno = "";
            if (_file != null && !string.IsNullOrEmpty(_nm_base))
            {
                using (var binaryReader = new BinaryReader(_file.InputStream))
                {
                    var fileParameter = new FileParameter(binaryReader.ReadBytes(_file.ContentLength), _file.FileName, _file.ContentType);
                    try
                    {
                        var doc = new Doc(_nm_base);
                        var dicionario = new Dictionary<string, object>();
                        dicionario.Add("file", fileParameter);
                        sRetorno = doc.incluir(dicionario);
                    }
                    catch (Exception ex)
                    {
                        throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
                    }
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