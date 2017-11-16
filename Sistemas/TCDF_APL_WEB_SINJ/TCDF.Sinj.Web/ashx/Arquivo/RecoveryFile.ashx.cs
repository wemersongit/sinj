using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for RecoveryFile
    /// </summary>
    public class RecoveryFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{}";
            var _id_file = context.Request["id_file"];
            var _ch_norma = context.Request["ch_norma"];
            var action = AcoesDoUsuario.arq_pro;
            var sAction = Util.GetEnumDescription(action) + ".RCP";

            SessaoUsuarioOV sessao_usuario = null;

            try
            {
                if (!string.IsNullOrEmpty(_id_file) && !string.IsNullOrEmpty(_ch_norma))
                {
                    sessao_usuario = Util.ValidarSessao();

                    var docRn = new Doc("sinj_arquivo_versionado_norma");
                    var docOv = docRn.doc(_id_file);

                    if (docOv.id_file != null)
                    {
                        var file = docRn.download(_id_file);
                        if (file != null && file.Length > 0)
                        {
                            var fileParameter = new FileParameter(file, docOv.filename, docOv.mimetype);
                            try
                            {
                                var doc = new Doc("sinj_norma");
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