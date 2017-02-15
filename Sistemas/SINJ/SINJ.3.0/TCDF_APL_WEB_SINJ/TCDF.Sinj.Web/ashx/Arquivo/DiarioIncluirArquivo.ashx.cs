using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using System.IO;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for DiarioIncluirArquivo
    /// </summary>
    public class DiarioIncluirArquivo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var action = AcoesDoUsuario.dio_inc;
            HttpPostedFile _file = context.Request.Files["file"];
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                using (var binaryReader = new BinaryReader(_file.InputStream))
                {
                    var fileParameter = new FileParameter(binaryReader.ReadBytes(_file.ContentLength), _file.FileName, _file.ContentType);
                    sRetorno = new DiarioRN().AnexarArquivo(fileParameter);
                }
                if (_file != null)
                {
                    var log_arquivo = new LogUpload
                    {
                        arquivo = JSON.Deserializa<ArquivoOV>(sRetorno)
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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